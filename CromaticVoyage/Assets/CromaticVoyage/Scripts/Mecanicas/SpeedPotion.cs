using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    [SerializeField] private float speedIncrease = 2f; // Fator de aumento da velocidade
    [SerializeField] private float duration = 5f; // Duração do efeito da poção

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Certifique-se de que o jogador tem a tag "Player"
        {
            PlayerControllerV2 player = collision.GetComponent<PlayerControllerV2>();
            if (player != null)
            {
                player.ApplySpeedBoost(speedIncrease, duration); // Aplica o boost
            }

            Destroy(gameObject); // Destrói a poção após o uso
        }
    }
}