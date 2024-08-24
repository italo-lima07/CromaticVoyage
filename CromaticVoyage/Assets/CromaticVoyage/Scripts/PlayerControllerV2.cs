using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rig;
    private bool isJumping;

    [SerializeField] private Animator animator;


    private Stack<Command> _playerCommands;
    private Vector2 _moveDirection;
    private BoxCollider2D playerCollider2D;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        /*_playerCommands = new Stack<Command>();*/
        isJumping = false;
        playerCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(_moveDirection.x));
        Jump();
        Move();
        Escaneando();
        Attack();
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
                Debug.Log("Ataque 1 executado");
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

    private IEnumerator ResetAttack()
    {
        // Aguarda um tempo para que a animação de ataque termine
        yield return new WaitForSeconds(0.5f); // Ajuste o tempo conforme a duração da sua animação

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

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }*/

    void Escaneando()
    {
        RaycastHit2D raio = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        DbLinha(transform.position, Vector2.down, 1f);
        if (raio.collider && raio.collider.IsTouching(playerCollider2D))
        {
            isJumping = false;
        }
    }
    
    
    private void DbLinha(Vector2 startPos, Vector2 dir, float tamanho)
    {
        Debug.DrawLine(startPos,startPos + dir * tamanho,Color.yellow,Time.deltaTime);
    }
}

