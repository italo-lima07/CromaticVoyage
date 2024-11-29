using UnityEngine;
using TMPro; // Importa o namespace do TextMeshPro

public class DisplayMessageOnContact : MonoBehaviour
{
    [Header("Message Settings")]
    [SerializeField] private TextMeshProUGUI messageText; // Referência ao componente de texto
    [SerializeField] private string message = "Você entrou na área!"; // Mensagem a ser exibida

    private void Start()
    {
        // Garante que a mensagem comece invisível
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Message Text não foi atribuído no Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Exibe o texto quando o player entra em contato
        if (other.CompareTag("Player") && messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Oculta o texto quando o player sai do contato
        if (other.CompareTag("Player") && messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}