using UnityEngine;

public class BarrierControl2D : MonoBehaviour
{
    private bool isBarrierActive = true; // Inicialmente, a barreira está ativa
    private SpriteRenderer barrierRenderer;
    private Collider2D barrierCollider; // Referência ao Collider2D para controlar o isTrigger

    void Start()
    {
        // Inicializa o SpriteRenderer e o Collider2D
        barrierRenderer = GetComponent<SpriteRenderer>();
        barrierCollider = GetComponent<Collider2D>();

        // Certifique-se de que o SpriteRenderer e o Collider2D estão presentes
        if (barrierRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing!");
        }

        if (barrierCollider == null)
        {
            Debug.LogError("Collider2D component is missing!");
        }
        else
        {
            Debug.Log("Barrier initialized as active.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered: " + other.gameObject.name); // Log para verificar o trigger

        if (other.CompareTag("AreaATK"))
        {
            ToggleBarrier();
        }
    }

    private void ToggleBarrier()
    {
        isBarrierActive = !isBarrierActive;

        // Altera a cor para feedback visual e ajusta o isTrigger
        if (isBarrierActive)
        {
            barrierRenderer.color = new Color(1, 1, 1, 1); // Opaco
            barrierCollider.isTrigger = false; // A barreira é sólida quando ativa
            Debug.Log("Barrier is now active.");
        }
        else
        {
            barrierRenderer.color = new Color(1, 1, 1, 0.5f); // Semitransparente
            barrierCollider.isTrigger = true; // O jogador pode passar pela barreira quando inativa
            Debug.Log("Barrier is now inactive.");
        }
    }
}