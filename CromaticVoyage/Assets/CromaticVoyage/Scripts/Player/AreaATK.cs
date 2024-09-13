using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaATK : MonoBehaviour
{
    private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<HealthEnemyTest>() != null)
        {
            HealthEnemyTest health = collider.GetComponent<HealthEnemyTest>();
            health.Damage(damage);
        }
    }
}
