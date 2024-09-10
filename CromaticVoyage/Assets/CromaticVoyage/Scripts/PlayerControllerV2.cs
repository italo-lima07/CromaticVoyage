using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour
{
    
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] public int maxHealth = 100; // Vida máxima do jogador
    public int currentHealth; // Vida atual do jogador
    private Rigidbody2D rig;
    private bool isJumping;
    public static bool IsAttack1Used = false;
    

    [SerializeField] private Animator animator;
    
    private GameObject attackArea = default;
    [SerializeField] private GameObject attackArea3;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private bool isOnDamageArea = false;

    private Stack<Command> _playerCommands;
    private Vector2 _moveDirection;
    private BoxCollider2D playerCollider2D;
    
    // Variáveis para o disparo do projétil
    [SerializeField] private GameObject bulletPrefab; // Prefab do projétil
    [SerializeField] private Transform firePoint; // Ponto de saída do projétil
    [SerializeField] private float bulletSpeed = 10f; // Velocidade do projétil
    [SerializeField] private float bulletLifetime = 2f; // Tempo de vida do projétil antes de se autodestruir

    // Variáveis para o cooldown do Ataque 3
    [SerializeField] private float attack3Cooldown = 2f; // Tempo de cooldown em segundos
    private float attack3CooldownTimer = 0f; // Tempo restante de cooldown
    
    public float knockbackForce = 10f; // Força do empurrão
    private float invulnerableTime = 1.0f; // Tempo de invulnerabilidade (1 segundo)
    private bool isInvulnerable = false;

    private bool noChao;
    
    public float resistanceMultiplier = 1f;
    [SerializeField] private GameObject shieldVisual; // ou public GameObject shieldVisual;
    public bool isShielded = false;
    private float shieldDuration = 5f; // duração do escudo
    void Start()
    {
        shieldVisual.SetActive(false); // desativa o visual do escudo no início
        rig = GetComponent<Rigidbody2D>();
        //isJumping = false;
        playerCollider2D = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;

        // Configuração das áreas de ataque
        attackArea = transform.GetChild(0).gameObject; // Primeiro filho do player
        attackArea.SetActive(false); // Desativando por padrão

        attackArea3 = transform.GetChild(1).gameObject; // Segundo filho do player
        attackArea3.SetActive(false); // Desativando por padrão

        // Certifique-se de que o firePoint é atribuído corretamente via Inspector ou código
        // firePoint não deve ser confundido com attackArea3
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            noChao = other.contacts[0].point.y < transform.position.y;
            if (noChao)
            {
                isJumping = false;
                animator.SetBool("IsJumping", false);
            }

            if (noChao && other.gameObject.CompareTag("Plataform"))
            {
                
                // Define o objeto do jogador como filho da plataforma
                transform.parent = other.collider.transform;
            }
            else
            {
                // Se não for uma plataforma, remove a paternidade (se já estiver definida)
                transform.parent = null;
            }
            
        }
        
        // Lógica de dano ao colidir com objetos de "Damage1"
        if (other.gameObject.CompareTag("Damage1"))
        {
            // Aplica dano e empurrão
            TakeDamage(10); // Ajuste o valor do dano conforme necessário
            ApplyKnockback(other.transform.position); // Aplica o empurrão baseado na posição da colisão
        }
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(_moveDirection.x));
        //Escaneando();
        Jump();
        Move();
        Attack();
        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0f;
                attacking = false;
                attackArea.SetActive(false);
                attackArea3.SetActive(false); // Desativa a área de ataque 3 após o tempo
                animator.ResetTrigger("IsAtk");
            }
        }
        
        // Atualiza o timer de cooldown para o Ataque 3
        if (attack3CooldownTimer > 0)
        {
            attack3CooldownTimer -= Time.deltaTime;
        }
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
            rig.AddForce(new Vector2(0, jumpForce * 2), ForceMode2D.Impulse);

            // Inicia uma corrotina para aguardar o final do pulo
           // EndJump();
        }
    }

    private void EndJump()
    {
        // Aguarda até que a velocidade vertical diminua, indicando que o pulo terminou
        while (rig.velocity.y > 0)
        {
            return; // Espera um frame
        }

        // Define o estado de pulo como falso e atualiza o Animator
        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

    public void Move()
    {
        _moveDirection = Input.GetAxis("Horizontal") * Vector2.right;
        
        if (_moveDirection.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_moveDirection.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("IsAtk");

            if (Input.GetKeyDown(KeyCode.J))
            {
                AttackLogic(attackArea);
                Debug.Log("Ataque 1 executado");
                IsAttack1Used = true;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Shoot();
                Debug.Log("Ataque 2 executado");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                if (attack3CooldownTimer <= 0)
                {
                    Debug.Log("Ataque 3 executado");
                    Attack3Logic(); // Lógica do Ataque 3
                    attack3CooldownTimer = attack3Cooldown; // Reseta o cooldown
                }
                else
                {
                    Debug.Log("Ataque 3 ainda está em cooldown");
                }
            }

            StartCoroutine(ResetAttack());
        }
    }
    
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Define a direção do projétil com base na direção que o jogador está virado
        bulletRb.velocity = new Vector2(transform.localScale.x * bulletSpeed, 0);

        // Destroi o projétil após um tempo
        Destroy(bullet, bulletLifetime);
    }
    
    private void AttackLogic(GameObject attackLocation)
    {
        attacking = true;
        attackLocation.SetActive(true); // Ativa a área de ataque apropriada
    }
    private void Attack3Logic()
    {
        attackArea3.SetActive(true); // Ativa a área de ataque 3
        attacking = true; // Define como atacando
        // Qualquer outra lógica relacionada ao ataque 3 pode ser adicionada aqui
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f); // Ajuste conforme necessário

        IsAttack1Used = false;
        attackArea3.SetActive(false); // Desativa a área de ataque 3 após o ataque
        animator.ResetTrigger("IsAtk");
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(_moveDirection.x * moveSpeed, rig.velocity.y);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;
    }
    private void Escaneando()
    {
        RaycastHit2D raio = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        DbLinha(transform.position, Vector2.down, 1f);

        if (raio.collider && raio.collider.IsTouching(playerCollider2D))
        {
            noChao = true;
        }
        else
        {
            noChao = false;
        }
        /*
        isJumping = false;
        animator.SetBool("IsJumping", false);

        // Verifica se o objeto colidido tem a tag "Plataform"
        if (raio.collider.CompareTag("Plataform"))
        {
            // Define o objeto do jogador como filho da plataforma
            transform.parent = raio.collider.transform;
        }
        else
        {
            // Se não for uma plataforma, remove a paternidade (se já estiver definida)
            transform.parent = null;
        }
    }
    else
    {
        // Se não estiver tocando em nada, remove a paternidade
        transform.parent = null;
    }*/
    }
    
    
    private void DbLinha(Vector2 startPos, Vector2 dir, float tamanho)
    {
        Debug.DrawLine(startPos,startPos + dir * tamanho,Color.yellow,Time.deltaTime);
    }
    
    // Método para aplicar dano
    public void TakeDamage(int damage)
    {
        if (!isInvulnerable) // Aplica o dano somente se não estiver invulnerável
        {
            if (isShielded) // Aplica resistência ao dano caso o escudo esteja ativo
            {
                damage = Mathf.FloorToInt(damage / resistanceMultiplier); // Reduz o dano com base no multiplicador
                Debug.Log("Escudo ativo! Dano reduzido.");
            }

            currentHealth -= damage; // Aplica o dano ao jogador
            Debug.Log("Jogador recebeu dano: " + damage);

            if (currentHealth <= 0)
            {
                Die(); // Executa a lógica de morte caso a vida chegue a zero
            }

            StartCoroutine(InvulnerabilityCooldown()); // Inicia o período de invulnerabilidade
        }
    }
    
    public void ActivateShield()
    {
        isShielded = true;
        shieldVisual.SetActive(true); // ativa o visual do escudo
        StartCoroutine(ShieldCooldown());
    }

    private IEnumerator ShieldCooldown()
    {
        yield return new WaitForSeconds(shieldDuration); // aguarda o tempo de duração do escudo
        isShielded = false;
        shieldVisual.SetActive(false); // desativa o visual do escudo
    }


    public void Die()
    {
        Debug.Log("Player morreu");

        // Verifica se o GameManager está inicializado
        if (GameManager.Instance != null)
        {
            // Notifica o GameManager para carregar a cena GameOver
            GameManager.Instance.PlayerDied();
        }
        else
        {
            Debug.LogWarning("GameManager não está inicializado.");
        }

        // Destrói o jogador
        Destroy(gameObject);
    }
    
    // Empurrão ao ser atingido
    private void ApplyKnockback(Vector3 damageSourcePosition)
    {
        Vector2 knockbackDirection = (transform.position - damageSourcePosition).normalized; // Direção oposta à fonte do dano
        rig.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // Aplica o empurrão
    }

    // Corrotina para invulnerabilidade temporária após receber dano
    private IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true; // Torna o jogador invulnerável
        yield return new WaitForSeconds(invulnerableTime); // Espera o tempo de invulnerabilidade
        isInvulnerable = false; // Volta a ser vulnerável
    }
}

