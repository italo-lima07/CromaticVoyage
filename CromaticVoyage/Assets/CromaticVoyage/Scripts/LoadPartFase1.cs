using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadPartFase1 : MonoBehaviour
{
    public string cenaParaCarregar;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no gatilho tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Carrega a cena chamada 
            SceneManager.LoadScene(cenaParaCarregar);
        }
    }
}
