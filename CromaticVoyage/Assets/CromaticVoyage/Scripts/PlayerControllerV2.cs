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
        Jump();
        Move();
        Escaneando();
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                rig.AddForce(new Vector2(0, jumpForce * 2), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
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

