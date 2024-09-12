using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyTest : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int MAX_HEALTH = 100;

    // Referência para o Animator
    private Animator animator;

    private void Start()
    {
        // Inicializa a referência ao Animator
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        // Toca a animação de morte
        animator.SetTrigger("GSBdie");

        // Aguarda o término da animação antes de destruir o inimigo
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Assume que a animação de morte tem o tempo de duração adequado
        // Espera o tempo da animação antes de destruir o objeto
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destrói o inimigo após a animação de morte
        Destroy(gameObject);
    }
}