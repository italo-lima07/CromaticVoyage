using UnityEngine;

public class LocalATK : MonoBehaviour
{
    [SerializeField] private int damage = 30;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HealthEnemyTest health = collider.GetComponent<HealthEnemyTest>();
        if (health != null)
        {
            // Passa a tag deste ataque para o inimigo, junto com o dano
            health.Damage(damage, gameObject.tag);
        }
        
        MalachaiHealth healthMalachai = collider.GetComponent<MalachaiHealth>();
        if (healthMalachai != null)
        {
            // Passa a tag deste ataque para o inimigo, junto com o dano
            healthMalachai.TakeDamage(damage, gameObject.tag);
        }
    }
}