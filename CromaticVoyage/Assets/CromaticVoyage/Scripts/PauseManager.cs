using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Inscreve no evento de cena carregada
        if (pauseMenu == null)
        {
            FindPauseMenu();
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false); // Garante que o menu começa desativado
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove a inscrição ao evento
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tenta encontrar o pauseMenu na nova cena
        FindPauseMenu();
    }

    private void FindPauseMenu()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("pausegame"); // Certifique-se de que o nome do objeto corresponde
        }

        if (pauseMenu == null)
        {
            Debug.LogWarning("PauseMenu não foi encontrado na cena atual.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenu == null) return;

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenu == null) return;

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Saiu do Jogo");
        Application.Quit();
    }
}