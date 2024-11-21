using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFase3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no gatilho tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Carrega a cena chamada "Fase3"
            SceneManager.LoadScene("Fase3");
        }
    }
}