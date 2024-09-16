using UnityEngine;

public class AreaATK : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Primeiro tenta pegar o componente HealthEnemyTest
        HealthEnemyTest healthEnemy = collider.GetComponent<HealthEnemyTest>();
        if (healthEnemy != null)
        {
            // Passa a tag deste ataque para o inimigo, junto com o dano
            healthEnemy.Damage(damage, gameObject.tag);
        }

        // Agora tenta pegar o componente HealthBoss
        HealthBoss healthBoss = collider.GetComponent<HealthBoss>();
        if (healthBoss != null)
        {
            // Passa o dano para o script HealthBoss
            healthBoss.TakeDamage(damage, gameObject.tag);
        }
        
        MalachaiHealth healthMalachai = collider.GetComponent<MalachaiHealth>();
        if (healthMalachai != null)
        {
            // Passa o dano para o script HealthBoss
            healthMalachai.TakeDamage(damage, gameObject.tag);
        }
    }
}
