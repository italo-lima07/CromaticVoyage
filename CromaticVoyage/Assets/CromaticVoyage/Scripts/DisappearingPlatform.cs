using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float disappearTime = 2f;  // Tempo até a plataforma desaparecer
    [SerializeField] private float reappearTime = 3f;   // Tempo até a plataforma reaparecer

    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Inicia a corrotina para fazer a plataforma desaparecer
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearTime);

        // Desativa o renderizador da plataforma para torná-la invisível
        platformRenderer.enabled = false;

        // Transforma o colisor em um trigger
        platformCollider.isTrigger = true;

        // Espera o tempo para reaparecer
        yield return new WaitForSeconds(reappearTime);

        // Reativa o renderizador da plataforma
        platformRenderer.enabled = true;

        // Retorna o colisor ao estado original
        platformCollider.isTrigger = false;
    }
}