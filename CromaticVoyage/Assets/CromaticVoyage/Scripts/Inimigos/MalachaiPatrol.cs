using UnityEngine;
using System.Collections;

public class MalachaiPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    //[SerializeField] private float secondPhaseSpeedMultiplier = 1.5f; // Multiplicador da segunda fase
    //private bool isInSecondPhase = false;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;
    

    private bool isStunned = false;
    private bool isDead = false;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("MovingGSB", false);
    }

    private void Update()
    {
        if (isDead || enemy == null) return;

        if (isStunned) return;

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("MovingGSB", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("MovingGSB", true);

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed; // Define a nova velocidade do boss (segunda fase)
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        anim.SetBool("MovingGSB", false);
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("MovingGSB", false);
        Destroy(gameObject);
    }
}