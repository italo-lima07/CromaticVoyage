using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20; // Valor do dano que a bala causa

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Primeiro tenta pegar o componente HealthEnemyTest
        HealthEnemyTest enemyHealth = collider.GetComponent<HealthEnemyTest>();
        if (enemyHealth != null)
        {
            // Causa dano ao inimigo, passando a tag da bala
            enemyHealth.Damage(damage, gameObject.tag);
        }

        // Agora tenta pegar o componente HealthBoss
        HealthBoss healthBoss = collider.GetComponent<HealthBoss>();
        if (healthBoss != null)
        {
            // Causa dano ao Boss
            healthBoss.TakeDamage(damage, gameObject.tag);
        }
        
        MalachaiHealth healthMalachai = collider.GetComponent<MalachaiHealth>();
        if (healthMalachai != null)
        {
            // Passa o dano para o script HealthBoss
            healthMalachai.TakeDamage(damage, gameObject.tag);
        }

        // Destrói a bala após colidir
        Destroy(gameObject);
    }
}