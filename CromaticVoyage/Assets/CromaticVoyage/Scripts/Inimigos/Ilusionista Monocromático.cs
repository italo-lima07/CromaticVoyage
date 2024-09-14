using UnityEngine;

public class IlusionistaMonocromático : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float closeRangeDamageCooldown; // Cooldown de dano por proximidade
    [SerializeField] private float closeRangeDamage; // Dano se o jogador se aproximar demais
    [SerializeField] private float rangedAttackDamage; // Dano do ataque à distância
    [SerializeField] private float movementSpeed;

    [Header("Range Parameters")]
    [SerializeField] private float closeRange; // Distância para aplicar dano se o jogador estiver perto
    [SerializeField] private float detectionRange; // Distância para detectar o jogador e se mover

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D closeRangeCollider; // Área de dano próximo
    [SerializeField] private BoxCollider2D detectionRangeCollider; // Área de detecção de jogador
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float closeRangeDamageTimer = Mathf.Infinity;
    private Animator anim;
    private PlayerControllerV2 playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        closeRangeDamageTimer += Time.deltaTime;

        if (PlayerInCloseRange())
        {
            if (closeRangeDamageTimer >= closeRangeDamageCooldown)
            {
                closeRangeDamageTimer = 0;
                DamagePlayerCloseRange();
            }
        }

        if (PlayerInDetectionRange())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                MoveTowardPlayer();
            }
        }
    }

    // Verifica se o jogador está dentro do alcance de ataque próximo
    private bool PlayerInCloseRange()
    {
        Collider2D hit = Physics2D.OverlapBox(closeRangeCollider.bounds.center, closeRangeCollider.bounds.size, 0, playerLayer);
        
        if (hit != null)
        {
            playerController = hit.GetComponent<PlayerControllerV2>();
            return playerController != null;
        }
        
        return false;
    }

    // Verifica se o jogador está dentro da área de detecção (longa distância)
    private bool PlayerInDetectionRange()
    {
        Collider2D hit = Physics2D.OverlapBox(detectionRangeCollider.bounds.center, detectionRangeCollider.bounds.size, 0, playerLayer);
        
        if (hit != null)
        {
            playerController = hit.GetComponent<PlayerControllerV2>();
            return playerController != null;
        }
        
        return false;
    }

    // Aplica dano ao jogador se ele estiver perto demais
    private void DamagePlayerCloseRange()
    {
        if (playerController != null)
        {
            playerController.TakeDamage((int)closeRangeDamage); // Converte para int
            Debug.Log("Jogador levou dano por estar muito perto!");
        }
    }

    // Move o inimigo rapidamente em direção ao jogador
    private void MoveTowardPlayer()
    {
        if (playerController != null)
        {
            Vector3 targetPosition = playerController.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            Debug.Log("Inimigo ilusionista está indo em direção ao jogador!");
        }
    }

    // Desenha as duas áreas de alcance no editor para facilitar a visualização
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Área de ataque próximo
        Gizmos.DrawWireCube(closeRangeCollider.bounds.center, closeRangeCollider.bounds.size);

        Gizmos.color = Color.blue; // Área de detecção de longa distância
        Gizmos.DrawWireCube(detectionRangeCollider.bounds.center, detectionRangeCollider.bounds.size);
    }
}
