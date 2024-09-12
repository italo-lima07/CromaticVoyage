using UnityEngine;

public class BarrierControl2D : MonoBehaviour
{
    private bool isBarrierActive = true; // Inicialmente, a barreira está ativa
    private BoxCollider2D barrierCollider;
    private SpriteRenderer barrierRenderer;

    void Start()
    {
        barrierCollider = GetComponent<BoxCollider2D>();
        barrierRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Verifica se o jogador usou o Ataque 1
        if (PlayerControllerV2.IsAttack1Used)
        {
            ToggleBarrier();
            PlayerControllerV2.IsAttack1Used = false; // Reseta o estado após o uso
        }
    }

    private void ToggleBarrier()
    {
        isBarrierActive = !isBarrierActive;

        // Ativa ou desativa o collider da barreira
        barrierCollider.enabled = isBarrierActive;

        // Dá feedback visual, como alterar a transparência
        if (isBarrierActive)
        {
            barrierRenderer.color = new Color(1, 1, 1, 1); // Opaco
        }
        else
        {
            barrierRenderer.color = new Color(1, 1, 1, 0.5f); // Semitransparente
        }
    }
}