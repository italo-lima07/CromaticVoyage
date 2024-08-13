using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rig;
    public bool isGrounded;
    private bool isJumping;

    private Stack<Command> _playerCommands;
    private Vector2 _moveDirection;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        _playerCommands = new Stack<Command>();
        isGrounded = false;
    }

    public void RegisterJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            _playerCommands.Push(new Jump(rig, jumpForce));
            _playerCommands.Peek().Do();
            isGrounded = false;
        }

        if (context.canceled)
        {
            rig.velocity = new Vector2(rig.velocity.x, 0);
        }
        
    }

    public void RegisterMove(InputAction.CallbackContext context)
    {
        _playerCommands.Push(new Move(context.ReadValue<Vector2>(), this));
        _playerCommands.Peek().Do();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A)) return;
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
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            isGrounded = false;
        }
    }
}

public abstract class Command
{
    public abstract void Do();
    public abstract void Undo();
}

public class Jump : Command
{
    private Rigidbody2D rig;
    private float jumpForce;

    public Jump(Rigidbody2D rb2d, float jump)
    {
        rig = rb2d;
        jumpForce = jump;
    }
    public override void Do()
    {
        rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Undo()
    {

    }
}

public class Move : Command
{
    private Vector2 direction;
    private PlayerController player;

    public Move(Vector2 dir, PlayerController play)
    {
        direction = dir;
        player = play;
    }

    public override void Do()
    {
        player.SetMoveDirection(direction);
    }

    public override void Undo()
    {

    }
}
  