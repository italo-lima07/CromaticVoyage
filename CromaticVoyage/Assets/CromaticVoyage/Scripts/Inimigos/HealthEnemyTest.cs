using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthEnemyTest : MonoBehaviour
{
    [SerializeField] private int health = 50;
    private int MAX_HEALTH = 50;

    // Referência para o Animator
    private Animator animator;

    [Header("Drop Settings")]
    [SerializeField] private GameObject healthPotionPrefab; // Prefab da poção de cura
    [SerializeField] private float dropChance = 0.25f; // 25% de chance de dropar

    [Header("Damage Settings")]
    [SerializeField] private List<string> damageTags; // Lista de tags que podem causar dano neste inimigo

    
    private void Start()
    {
        // Inicializa a referência ao Animator
        animator = GetComponent<Animator>();
        
    }

    public void Damage(int amount, string attackTag)
    {
        // Verifica se a tag do ataque é permitida para este inimigo
        if (!damageTags.Contains(attackTag))
        {
            return; // Não aplica dano se a tag não estiver na lista
        }

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        // Toca a animação de hit
        animator.SetTrigger("GSBhit");
        
        // Tocar o som de dano "hitenemy"
        AudioObserver.OnPlaySfxEvent("hitenemy");

        this.health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Toca a animação de morte
        animator.SetTrigger("GSBdie");

        // Desativa o EnemyPatrol se ele existir
        EnemyPatrol enemyPatrol = GetComponentInParent<EnemyPatrol>();
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false;
        }

        // Aguarda o término da animação antes de destruir o inimigo
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Espera o tempo da animação de morte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Tenta spawnar a poção de cura
        TrySpawnHealthPotion();

        // Destrói o inimigo após a animação de morte
        Destroy(gameObject);
    }

    private void TrySpawnHealthPotion()
    {
        // Calcula aleatoriamente se a poção será dropada
        if (Random.value <= dropChance)
        {
            // Spawna a poção de cura na posição atual do inimigo
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
        }
    }
    
    // Verifica colisões com projéteis
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageTags.Contains(collision.gameObject.tag))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Damage(bullet.damage, collision.gameObject.tag); // Passa a tag da bala
            }
        }
    }
}
