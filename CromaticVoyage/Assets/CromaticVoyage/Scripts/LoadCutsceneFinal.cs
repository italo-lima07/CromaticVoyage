using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadCutsceneFinal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no gatilho tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Carrega a cena chamada "final"
            SceneManager.LoadScene("CutSceneFinal");
        }
    }
}
