using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private float lerpSpeed = 0.1f;

    private PlayerControllerV2 playerController;

    private void Start()
    {
        // Obtém a referência do PlayerControllerV2
        playerController = FindObjectOfType<PlayerControllerV2>();

        if (playerController == null)
        {
            Debug.LogError("PlayerControllerV2 não encontrado! Verifique se há um PlayerControllerV2 na cena.");
        }
        
        // Inicializa a barra de vida com a vida atual do player
        if (playerController != null)
        {
            healthSlider.maxValue = playerController.maxHealth;
            healthSlider.value = playerController.currentHealth;
            easeHealthSlider.maxValue = playerController.maxHealth;
            easeHealthSlider.value = playerController.currentHealth;
        }
    }

    private void Update()
    {
        if (playerController != null)
        {
            float health = playerController.currentHealth;

            if (healthSlider.value != health)
            {
                healthSlider.value = health;
            }

            if (easeHealthSlider.value != health)
            {
                easeHealthSlider.value = math.lerp(easeHealthSlider.value, health, lerpSpeed);
            }
        }
    }
}