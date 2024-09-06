using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWater : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Acessa o script de vida do player para matar o jogador
            PlayerControllerV2 player = other.GetComponent<PlayerControllerV2>();

            if (player != null)
            {
                player.Die();
            }
            else
            {
                Debug.LogWarning("Referência ao PlayerControllerV2 não encontrada.");
            }
        }
    }
}

