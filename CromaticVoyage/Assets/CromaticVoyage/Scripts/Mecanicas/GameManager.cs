using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string lastScene; // Armazena a última cena em que o jogador estava

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    // Update is called once per frame
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Initialization")
        {
            SceneManager.LoadScene("Splash");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void PlayerDied()
    {
        // Armazena o nome da cena atual antes de carregar o GameOver
        lastScene = SceneManager.GetActiveScene().name;

        // Carrega a cena de Game Over
        SceneManager.LoadScene("GameOver");
    }

    void Update()
    {
        // Verifica se estamos na cena GameOver e o jogador apertou "Enter"
        if (SceneManager.GetActiveScene().name == "GameOver" && Input.GetKeyDown(KeyCode.Return))
        {
            // Volta para a última cena onde o jogador estava
            if (!string.IsNullOrEmpty(lastScene))
            {
                SceneManager.LoadScene(lastScene);
            }
        }
    }
}
