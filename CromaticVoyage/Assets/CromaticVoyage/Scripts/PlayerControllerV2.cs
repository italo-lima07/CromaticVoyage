using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int maxHealth = 100; // Vida máxima do jogador
    private int currentHealth; // Vida atual do jogador
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

    private bool noChao;
    void Start()
    {
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
        
        // Aplicar dano se o jogador estiver em uma área de dano
        if (isOnDamageArea)
        {
            TakeDamage(10); // Ajuste o valor do dano conforme necessário
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
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage1"))
        {
            isOnDamageArea = true;
            Debug.Log("Jogador entrou na área de dano");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage1"))
        {
            isOnDamageArea = false;
            Debug.Log("Jogador saiu da área de dano");
        }
    }*/
}

