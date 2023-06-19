using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float detectionRange = 10.0f;
    public float speed = 5.0f;
    public float wanderInterval = 2.0f;
    public float distanceToPlayer = 2.0f;

    public Transform playerTransform;
    public Transform enemyTransform;
    private bool playerDetected = false;
    private float wanderTimer = 0.0f;
    private Vector2 wanderDirection;
    private Animator animator;
    private Rigidbody2D rb;
    private WarriorStatus warriorStatus;
    private EnemyAttackManager attackManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTransform = transform;
        wanderDirection = GetRandomWanderDirection();
        animator = GetComponent<Animator>();
        warriorStatus = FindObjectOfType<WarriorStatus>();
        attackManager = gameObject.AddComponent<EnemyAttackManager>();
        attackManager = GetComponent<EnemyAttackManager>();

    }

    void Update()
    {
        attackManager.UpdateAttack(playerTransform);

        if (attackManager.enemyMove)
        {
            if (playerDetected)
            {
                ChasePlayer();
            }
            else
            {
                Wander();
            }
        }

        rb.velocity = Vector2.zero;
    }

    private void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer < 0.0f)
        {
            wanderDirection = GetRandomWanderDirection();
            wanderTimer = wanderInterval;
        }

        transform.Translate(wanderDirection * speed * Time.deltaTime);
        animator.SetFloat("EnemyMoveX", wanderDirection.x);
        animator.SetFloat("EnemyMoveY", wanderDirection.y);
        animator.SetBool("isEnemyMove", true);

        if (Vector2.Distance(transform.position, playerTransform.position) < detectionRange)
        {
            playerDetected = true;
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = playerTransform.position - enemyTransform.position;
        float distance = Vector2.Distance(enemyTransform.position, playerTransform.position);

        if (distance > distanceToPlayer)
        {
            enemyTransform.Translate(direction.normalized * speed * Time.deltaTime);
            animator.SetFloat("EnemyMoveX", direction.x);
            animator.SetFloat("EnemyMoveY", direction.y);
            animator.SetBool("isEnemyMove", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attackManager.StartAttack(other.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other == null) return;

        if (other.CompareTag("Player"))
        {

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Enemy_attack") && stateInfo.normalizedTime >= attackManager.count)
            {
                attackManager.ResetAttackDirection();
                attackManager.count += 1;
                UnityEngine.Debug.Log("공격방향 재설정");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attackManager.StopAttack();
        }
    }
    // 여기 오류만 발생하고  지워도 문제없길래 일단은 주석화해둠
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    attackManager.OnCollisionEnter2D(collision);
    //}

    private Vector2 GetRandomWanderDirection()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }

}
