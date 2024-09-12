using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 10; // Valor do dano que a bala causa

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthEnemyTest enemyHealth = collision.gameObject.GetComponent<HealthEnemyTest>();
        if (enemyHealth != null)
        {
            enemyHealth.Damage(damageAmount); // Causa dano ao inimigo
        }
        Destroy(gameObject); // Destroi a bala após a colisão
    }
}

