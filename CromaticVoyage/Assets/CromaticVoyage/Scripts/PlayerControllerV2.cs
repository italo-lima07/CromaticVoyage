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

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        /*_playerCommands = new Stack<Command>();*/
        isJumping = false;
    }

    private void Update()
    {
        Jump();
        Move();
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                //anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0,jumpForce * 2), ForceMode2D.Impulse);
                isJumping = true;
            }
            
            /*if (Input.GetButtonUp("Jump"))
            {
                rig.velocity = new Vector2(rig.velocity.x, 0);
            }*/
            
        }
        
    }

    public void Move()
    {
        /*_playerCommands.Push(new Move(context.ReadValue<Vector2>(), this));
        _playerCommands.Peek().Do();*/
        _moveDirection = Input.GetAxis("Horizontal") * Vector2.right;
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(_moveDirection.x * moveSpeed, rig.velocity.y);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 )
        {
            isJumping = false;
        }
    }

    
}
