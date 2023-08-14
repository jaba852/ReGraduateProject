
using System.Diagnostics;
using UnityEngine;

public class DeepOneMovement : MonoBehaviour
{
    public float detectionRange = 10f; // 적군 감지 범위
    public float attackRange = 5f; // 공격 사거리
    public float moveSpeed = 3f; // 이동 속도
    public float attackDelay = 2f; // 공격 딜레이

    public LayerMask wallLayer; // 벽 레이어

    private Transform player; // 플레이어의 Transform 컴포넌트
    private bool isAttacking = false; // 현재 공격 중인지 여부
    private float attackTimer = 0f; // 공격 딜레이 타이머

    private Animator animator;

    private DeepOneAttackManager attackManager;

    private bool enemydead = false;

    private bool PlayerFind = false;
    public GameObject EnemyFind;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackManager = GetComponent<DeepOneAttackManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 플레이어 감지 및 공격
        if (!isAttacking && !enemydead && !animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_reaction") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_stun"))
        {


            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange || PlayerFind)
            {
                // 적군이 플레이어를 감지하고 벽에 가려지지 않은 경우에만 이동
                if (distanceToPlayer <= attackRange && !enemydead)
                {
                    // 플레이어가 공격 사거리 내에 있으면 공격 실행
                    PlayerFind = true;
                    Attack();
                    UnityEngine.Debug.Log("플레이어감지");
                }
                else if (!IsWallBetweenEnemyAndPlayer())
                {
                    PlayerFind = true;
                    MoveTowardsPlayer();
                }
                else
                {
                    animator.SetBool("isEnemyMove", false);
                }
            }
            
        }
        else
        {
            // 공격 중일 때 타이머 갱신
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDelay)
            {
                // 공격 딜레이 후 공격 완료
                animator.SetBool("isEnemyAttack", false);
                isAttacking = false;
                attackTimer = 0f;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // 플레이어 쪽으로 이동
        if (PlayerFind)
        {
            EnemyFind.SetActive(true);

        }
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        Vector2 direction = player.position - transform.position;
        animator.SetFloat("EnemyMoveX", direction.x);
        animator.SetFloat("EnemyMoveY", direction.y);
        animator.SetBool("isEnemyMove", true);
    }

    private void Attack()
    {
        // 공격 실행
        if (PlayerFind)
        {
            EnemyFind.SetActive(true);

        }
        UnityEngine.Debug.Log("DeepOne attacks!");
        Vector2 direction = player.position - transform.position;
    
            animator.SetFloat("EnemyMoveX", direction.x);
            animator.SetFloat("EnemyMoveY", direction.y);
        

        animator.SetBool("isEnemyAttack", true);
        // 여기에 공격에 관련된 코드를 작성
        // 예를 들어, 탄환을 발사하거나 플레이어에게 데미지를 입힐 수 있음


        isAttacking = true;
    }

    public void SetEnemyDead()
    {
        enemydead = true;

        UnityEngine.Debug.Log("사망"+ enemydead);
    }

    private bool IsWallBetweenEnemyAndPlayer()
    {
        // 적군과 플레이어 사이에 벽이 있는지 체크
        Vector2 direction = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, wallLayer);

        if (hit.collider != null)
        {
            // 레이캐스트가 벽과 충돌한 경우
            return true;
        }

        // 벽이 없거나, 감지 범위 내에 벽이 없는 경우
        return false;
    }
}