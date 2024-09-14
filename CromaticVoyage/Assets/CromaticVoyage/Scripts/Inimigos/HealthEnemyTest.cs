using UnityEngine;
using System.Collections;

public class HealthEnemyTest : MonoBehaviour
{
    [SerializeField] private int health = 50;
    private int MAX_HEALTH = 50;

    // Referência para o Animator
    private Animator animator;
    
    [Header("Drop Settings")]
    [SerializeField] private GameObject healthPotionPrefab; // Prefab da poção de cura
    [SerializeField] private float dropChance = 0.25f; // 25% de chance de dropar

    private void Start()
    {
        // Inicializa a referência ao Animator
        animator = GetComponent<Animator>();
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        // Toca a animação de hit
        animator.SetTrigger("GSBhit");

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

    // Método que detecta colisões com qualquer ataque
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se colidiu com um objeto com a tag "PlayerAttack" ou "Projectile"
        if (collision.CompareTag("PlayerAttack") || collision.CompareTag("Projectile"))
        {
            // Pode ajustar a quantidade de dano com base no tipo de ataque
            int damageAmount = 10; // Coloque o valor adequado aqui

            Damage(damageAmount);
        }
    }
}
