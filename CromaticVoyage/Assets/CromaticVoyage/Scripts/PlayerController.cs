using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask groundLayer; // Configuração do Layer para o chão

    private Rigidbody2D rig;
    private bool isGrounded;
    private Stack<Command> _playerCommands;
    private Vector2 _moveDirection;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        _playerCommands = new Stack<Command>();
    }

    void Update()
    {
        // Aqui você pode adicionar qualquer lógica adicional de atualização, se necessário
    }

    private void FixedUpdate()
    {
        // Aplica a velocidade horizontal
        rig.velocity = new Vector2(_moveDirection.x * moveSpeed, rig.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((groundLayer & 1 << other.gameObject.layer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((groundLayer & 1 << other.gameObject.layer) != 0)
        {
            isGrounded = false;
        }
    }

    public void RegisterJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            _playerCommands.Push(new Jump(rig, jumpForce));
            _playerCommands.Peek().Do();
        }
    }

    public void RegisterMove(InputAction.CallbackContext context)
    {
        _playerCommands.Push(new Move(context.ReadValue<Vector2>(), this));
        _playerCommands.Peek().Do();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;
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
        // Implementar se necessário
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
        // Implementar se necessário
    }
}
    