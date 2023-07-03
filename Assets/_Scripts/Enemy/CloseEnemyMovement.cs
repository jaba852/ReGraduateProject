using System.Diagnostics;
using UnityEngine;

public class CloseEnemyMovement : MonoBehaviour
{
    public float detectionRange = 10f; // ���� ���� ����
    public float attackRange = 5f; // ���� ��Ÿ�
    public float moveSpeed = 3f; // �̵� �ӵ�
    public float attackDelay = 2f; // ���� ������

    public LayerMask wallLayer; // �� ���̾�

    private Transform player; // �÷��̾��� Transform ������Ʈ
    private bool isAttacking = false; // ���� ���� ������ ����
    private float attackTimer = 0f; // ���� ������ Ÿ�̸�

    private Animator animator;

    private CloseEnemyAttackManager attackManager;

    private bool enemydead = false;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackManager = GetComponent<CloseEnemyAttackManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // �÷��̾� ���� �� ����
        if (!isAttacking && !enemydead)
        {


            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
            {
                // ������ �÷��̾ �����ϰ� ���� �������� ���� ��쿡�� �̵�
                if (!IsWallBetweenEnemyAndPlayer())
                {
                    MoveTowardsPlayer();
                }
                else
                {
                    animator.SetBool("isEnemyMove", false);
                }
            }
            else if (distanceToPlayer <= attackRange && !enemydead)
            {
                // �÷��̾ ���� ��Ÿ� ���� ������ ���� ����
                Attack();
                UnityEngine.Debug.Log("�÷��̾��");
            }
        }
        else
        {
            // ���� ���� �� Ÿ�̸� ����
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDelay)
            {
                // ���� ������ �� ���� �Ϸ�
                animator.SetBool("isEnemyAttack", false);
                isAttacking = false;
                attackTimer = 0f;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // �÷��̾� ������ �̵�
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        Vector2 direction = player.position - transform.position;
        animator.SetFloat("EnemyMoveX", direction.x);
        animator.SetFloat("EnemyMoveY", direction.y);
        animator.SetBool("isEnemyMove", true);
    }

    private void Attack()
    {
        // ���� ����
        UnityEngine.Debug.Log("Enemy attacks!");
        Vector2 direction = player.position - transform.position;

        animator.SetFloat("EnemyMoveX", direction.x);
        animator.SetFloat("EnemyMoveY", direction.y);


        animator.SetBool("isEnemyAttack", true);
        // ���⿡ ���ݿ� ���õ� �ڵ带 �ۼ�
        // ���� ���, źȯ�� �߻��ϰų� �÷��̾�� �������� ���� �� ����


        isAttacking = true;
    }

    public void SetEnemyDead()
    {
        enemydead = true;

        UnityEngine.Debug.Log("���" + enemydead);
    }

    private bool IsWallBetweenEnemyAndPlayer()
    {
        // ������ �÷��̾� ���̿� ���� �ִ��� üũ
        Vector2 direction = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, wallLayer);

        if (hit.collider != null)
        {
            // ����ĳ��Ʈ�� ���� �浹�� ���
            return true;

        }

        // ���� ���ų�, ���� ���� ���� ���� ���� ���
        return false;
    }
}