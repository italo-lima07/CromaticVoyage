using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject MenuInicial;
    [SerializeField] private GameObject Opcoes;

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevelDeJogo);
    }

    public void AbrirOpcoes()
    {
        MenuInicial.SetActive(false);
        Opcoes.SetActive(true);
    }
    
    public void FecharOpcoes()
    {
        Opcoes.SetActive(false);
        MenuInicial.SetActive(true);
    }
    
    public void SairJogo()
    {
        Debug.Log("sair do Jogo");
        Application.Quit();
    }
}
