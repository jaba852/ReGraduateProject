using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEnemyMovement : MonoBehaviour
{
    public float detectionRange = 10.0f;
    public float speed = 5.0f;
    public float wanderInterval = 2.0f;
    public float distanceToPlayer = 2.0f;

    public Transform playerTransform;
    public Transform enemyTransform;
    private bool playerDetected = false;
    private float wanderTimer = 10.0f;
    private Vector2 wanderDirection;
    private Animator animator;
    private Rigidbody2D rb;
    private WarriorStatus warriorStatus;
    private CloseEnemyAttackManager attackManager;

    // Added variables for player detection delay
    private bool isPlayerDetectionDelayed = false;
    private float playerDetectionDelay = 2.0f;
    private float playerDetectionTimer = 0.0f;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        enemyTransform = transform;
        wanderDirection = GetRandomWanderDirection();
        animator = GetComponent<Animator>();
        warriorStatus = FindObjectOfType<WarriorStatus>();
        attackManager = GetComponent<CloseEnemyAttackManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start the delay timer
        playerDetectionTimer = playerDetectionDelay;
    }

    void Update()
    {
        // If player detection is delayed, reduce the timer
        if (isPlayerDetectionDelayed)
        {
            playerDetectionTimer -= Time.deltaTime;

            // If the timer reaches 0, try to find the player again
            if (playerDetectionTimer <= 0.0f)
            {
                FindPlayer();
                isPlayerDetectionDelayed = false;
            }
        }

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
        if (playerTransform == null)
        {
            // Player transform is missing, delay player detection
            isPlayerDetectionDelayed = true;
            playerDetectionTimer = playerDetectionDelay;
            return;
        }

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

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            // Player not found, delay player detection again
            isPlayerDetectionDelayed = true;
            playerDetectionTimer = playerDetectionDelay;
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

    private Vector2 GetRandomWanderDirection()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }
}