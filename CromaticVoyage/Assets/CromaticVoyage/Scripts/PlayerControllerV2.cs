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
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private bool isOnDamageArea = false;

    private Stack<Command> _playerCommands;
    private Vector2 _moveDirection;
    private BoxCollider2D playerCollider2D;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        isJumping = false;
        playerCollider2D = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        // Configuração da área de ataque
        attackArea = transform.GetChild(0).gameObject; // Supondo que a área de ataque seja o primeiro filho do player
        attackArea.SetActive(false); // Certifique-se de que a área de ataque esteja desativada por padrão
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(_moveDirection.x));
        Jump();
        Move();
        Escaneando();
        Attack();
        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0f;
                attacking = false;
                attackArea.SetActive(false);
                animator.ResetTrigger("IsAtk");
            }
        }
        
        // Aplicar dano se o jogador estiver em uma área de dano
        if (isOnDamageArea)
        {
            TakeDamage(10); // Ajuste o valor do dano conforme necessário
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
            StartCoroutine(EndJump());
        }
    }

    private IEnumerator EndJump()
    {
        // Aguarda até que a velocidade vertical diminua, indicando que o pulo terminou
        while (rig.velocity.y > 0)
        {
            yield return null; // Espera um frame
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

            // Aqui você pode adicionar lógica para cada ataque específico
            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetTrigger("IsAtk");
                AttackLogic();
                Debug.Log("Ataque 1 executado");
                IsAttack1Used = true;

                // Inicia a lógica de reset de ataque (opcional, já está coberta pelo Update)
                //StartCoroutine(ResetAttack());
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Ataque 2 executado");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Ataque 3 executado");
            }

            // Inicia a corrotina para resetar o parâmetro de ataque
            StartCoroutine(ResetAttack());
        }
    }
    
    private void AttackLogic()
    {
        attacking = true;
        attackArea.SetActive(true);
    }

    private IEnumerator ResetAttack()
    {
        // Aguarda um tempo para que a animação de ataque termine
        yield return new WaitForSeconds(0.5f); // Ajuste o tempo conforme a duração da sua animação

        IsAttack1Used = false;
        // Reseta o parâmetro de ataque
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
            isJumping = false;

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
        }
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

