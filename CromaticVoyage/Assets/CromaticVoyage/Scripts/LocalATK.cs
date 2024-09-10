using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalATK : MonoBehaviour
{
    private int damage = 20;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<HealthEnemyTest>() != null)
        {
            HealthEnemyTest health = collider.GetComponent<HealthEnemyTest>();
            health.Damage(damage);
        }
    }
}
