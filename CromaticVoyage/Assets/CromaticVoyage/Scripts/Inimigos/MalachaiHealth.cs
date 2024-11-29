using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro; // Importa o namespace do TextMeshPro
public class MalachaiHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;
    private int health;
    
    public Action<string> OnEnemyDefeated;
    
    public Action OnPhaseTwo; // Observer para a segunda fase
    private bool isPhaseTwo = false;
    
    [Header("Invulnerability Settings")]
    [SerializeField] private float invulnerabilityDuration = 5f;
    [SerializeField] private Collider2D bossCollider; // Collider 2D do boss
    
    [Header("Boss Speed Settings")]
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float phaseTwoSpeed = 4f;
    private MalachaiPatrol bossPatrol;
    
    private Collider2D enemyCollider; // Para ativar/desativar o trigger

    // Referência para o Animator
    private Animator animator;

    [Header("Drop Settings")]
    [SerializeField] private GameObject healthPotionPrefab; // Prefab da poção de cura
    [SerializeField] private float dropChance = 0.25f; // 25% de chance de dropar

    [Header("Damage Settings")]
    [SerializeField] private List<string> damageTags; // Lista de tags que podem causar dano neste inimigo

    [SerializeField] private BarraDeVidaBosses _healthBar;
    
    [Header("Death Message Settings")]
    [SerializeField] private TextMeshProUGUI deathMessageText; // Referência ao texto de mensagem
    [SerializeField] private string deathMessage = "Você derrotou o Malachai! Agora dê um ataque básico, apertando J."; // Texto exibido ao derrotar o inimigo
    [SerializeField] private float messageDuration = 2f; // Duração da exibição do texto
    private void Start()
    {
        // Inicializa a vida para o valor máximo
        health = maxHealth;
        _healthBar.UpdateHealthBar(health, maxHealth);
        // Inicializa a referência ao Animator
        animator = GetComponent<Animator>();
        _healthBar = GetComponentInChildren<BarraDeVidaBosses>();
        // Certifica-se de que o Collider2D está sendo atribuído
        enemyCollider = GetComponent<Collider2D>();
        
        bossPatrol = GetComponentInParent<MalachaiPatrol>();
        
        if (deathMessageText != null)
        {
            deathMessageText.gameObject.SetActive(false); // Garante que o texto comece invisível
        }
        
        // Inicia com o boss invulnerável
        StartCoroutine(InvulnerabilityRoutine());
        
    }
    private IEnumerator InvulnerabilityRoutine()
    {
        // Ativa a invulnerabilidade ao começar
        bossCollider.isTrigger = false;
        yield return new WaitForSeconds(invulnerabilityDuration);
        // Desativa a invulnerabilidade após o tempo definido
        bossCollider.isTrigger = true;
    }
    


    public void TakeDamage(int amount, string attackTag)
    {
        // Verifica se a tag do ataque é permitida para este inimigo
        if (!damageTags.Contains(attackTag))
        {
            return; // Não aplica dano se a tag não estiver na lista
        }
        
        // Checa se o boss está invulnerável (isTrigger desativado) ou se a tag do ataque não é permitida
        if (!bossCollider.isTrigger || !damageTags.Contains(attackTag))
            return;

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        // Toca a animação de hit
        animator.SetTrigger("GSBhit");
        
        // Tocar o som de dano "hitenemy"
        AudioObserver.OnPlaySfxEvent("hitenemy");

        this.health -= amount;
        _healthBar.UpdateHealthBar(health, maxHealth);
        
        if (health <= 0)
        {
            Die();
        }
        else if (!isPhaseTwo && health <= maxHealth / 2) // Checa se a vida caiu abaixo de 50% para mudar de fase
        {
            ActivatePhaseTwo();
        }
    }
    
    private void ActivatePhaseTwo()
    {
        isPhaseTwo = true;
        OnPhaseTwo?.Invoke(); // Notifica a segunda fase
        bossPatrol.SetSpeed(phaseTwoSpeed); // Aumenta a velocidade do boss na segunda fase
    }

    private void Die()
    {
        //OnEnemyDefeated?.Invoke(gameObject.tag);
        // Toca a animação de morte
        animator.SetTrigger("GSBdie");

        // Desativa o EnemyPatrol se ele existir
        MalachaiPatrol enemyPatrol = GetComponentInParent<MalachaiPatrol>();
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
        
        if (deathMessageText != null)
        {
            StartCoroutine(ShowDeathMessage());
        }

        // Destrói o inimigo após a animação de morte
        Destroy(gameObject);
    }
    
    private IEnumerator ShowDeathMessage()
    {
        deathMessageText.gameObject.SetActive(true);
        deathMessageText.text = deathMessage;

        yield return new WaitForSeconds(messageDuration);

        deathMessageText.gameObject.SetActive(false);
    }

    private void TrySpawnHealthPotion()
    {
        // Calcula aleatoriamente se a poção será dropada
        if (UnityEngine.Random.value <= dropChance)
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
                TakeDamage(bullet.damage, collision.gameObject.tag); // Passa a tag da bala
            }
        }
    }
}
