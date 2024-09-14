using System.Collections;
using UnityEngine;
using TMPro; // Importa o TextMeshPro namespace

public class HealingPotion : MonoBehaviour
{
    public int healingAmount = 20; // Quantidade de vida restaurada pela poção
    public TextMeshProUGUI warningText; // Referência ao TMP Text para a mensagem

    private void Start()
    {
        // Verifica se o warningText foi atribuído
        if (warningText == null)
        {
            Debug.LogError("Warning Text não foi atribuído no Inspector.");
        }
        else
        {
            // Garante que a mensagem comece invisível
            warningText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerV2 player = other.GetComponent<PlayerControllerV2>();

            if (player != null)
            {
                // Verifica se o jogador já está com a vida cheia
                if (player.currentHealth < player.maxHealth)
                {
                    // Garante que o jogador não ultrapasse a vida máxima
                    player.currentHealth = Mathf.Min(player.maxHealth, player.currentHealth + healingAmount);

                    // Aqui você pode adicionar efeitos sonoros, animação ou feedback visual

                    Destroy(gameObject); // Remove a poção do jogo após o uso
                }
                else if (warningText != null) // Certifica-se de que warningText não seja nulo
                {
                    // Exibe a mensagem de "Vida cheia" na tela
                    StartCoroutine(ShowWarningMessage());
                }
            }
        }
    }

    private IEnumerator ShowWarningMessage()
    {
        // Ativa o texto de aviso, se não for nulo
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            warningText.text = "Vida cheia! Não pode usar a poção.";

            // Espera por 2 segundos
            yield return new WaitForSeconds(2f);

            // Desativa o texto de aviso
            warningText.gameObject.SetActive(false);
        }
    }
}
