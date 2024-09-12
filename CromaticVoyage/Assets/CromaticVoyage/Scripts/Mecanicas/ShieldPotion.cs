using System.Collections;
using UnityEngine;

public class ShieldPotion : MonoBehaviour
{
    public float shieldDuration = 5f; // Duração do escudo
    public float chanceToDoubleResistance = 0.5f; // 50% de chance para dobrar resistência
    private bool shieldActive = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControllerV2>().ActivateShield();
            Destroy(gameObject); // Destrói a poção após a coleta
        }
    }


    private IEnumerator ActivateShield(PlayerControllerV2 player)
    {
        shieldActive = true;
        player.isShielded = true;

        // Verifica se o efeito de dobrar a resistência ocorre
        if (Random.value < chanceToDoubleResistance)
        {
            player.resistanceMultiplier = 2f; // Dobra a resistência
            Debug.Log("Resistência dobrada!");
        }
        else
        {
            player.resistanceMultiplier = 1f; // Normal
        }

        // Exibe um feedback visual do escudo (opcional)
        // Você pode adicionar uma animação ou alterar a cor do jogador

        // Aguarda a duração do escudo
        yield return new WaitForSeconds(shieldDuration);

        // Reseta as variáveis após o escudo acabar
        player.isShielded = false;
        player.resistanceMultiplier = 1f;
        shieldActive = false;

        Destroy(gameObject); // Destroi a poção após o uso
    }
}