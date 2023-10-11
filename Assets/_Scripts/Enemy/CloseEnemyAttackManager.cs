using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class CloseEnemyAttackManager : MonoBehaviour
{
    private Animator animator;
    public bool enemyMove = true;
    public bool attackStop = false;
    public bool enemydead = false;
    public int count = 1;
    private Rigidbody2D rb;
    private WarriorStatus warriorStatus;



    public float attackRange = 2.0f; // ���� �Ÿ�

    private Transform playerTransform;   // �÷��̾� Ʈ������

    private Vector2 targetPosition;     // �÷��̾� ĳ���� ��ġ

    public AudioClip EnemyattacksoundClip; // Enemy ���� ���� Ŭ��
    public AudioClip PlayerHitsoundClip; // �ǰ� ���� Ŭ��
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ
    private bool isEnemyAttackSoundPlaying = false; // Enemy ���� ���� ��� ���θ� �����ϴ� ����
    private bool isPlayerHitSoundPlaying = false; // �ǰ� ���� ��� ���θ� �����ϴ� ����





    public void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        warriorStatus = FindObjectOfType<WarriorStatus>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform; // ã�� ���� ������Ʈ�� Transform�� �����ɴϴ�.
        }

        // ���� ������Ʈ�� AudioSource ������Ʈ�� �߰�
        audioSource = GetComponent<AudioSource>();

    }

    public void UpdateAttack(Transform playerTransform)
    {
     

        if (attackStop == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Enemy_attack") && stateInfo.normalizedTime >= 1.0f)
            {
                enemyMove = true;
                animator.SetBool("isEnemyAttack", false);
                UnityEngine.Debug.Log("isEnemyAttack, false");
            }
        }
    }

    public void StartAttack(Transform playerTransform)
    {
        UnityEngine.Debug.Log("Warrior�� �߰�!");
        Vector2 direction = playerTransform.position - transform.position;
        if (enemydead == false)
        {
            animator.SetFloat("EnemyMoveX", direction.x);
            animator.SetFloat("EnemyMoveY", direction.y);
        }

        animator.SetBool("isEnemyAttack", true);
        rb.velocity = new Vector2(0, 0);
        enemyMove = false;
        attackStop = false;
    }


    public void StopAttack()
    {
        if (enemydead == false)
        {
            attackStop = true;
            count = 1;
            UnityEngine.Debug.Log("count �ʱ�ȭ");
        }
    }

    public void ResetAttackDirection()
    {
        Vector2 direction = playerTransform.position - transform.position;
        animator.SetFloat("EnemyMoveX", direction.x);
        animator.SetFloat("EnemyMoveY", direction.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!");
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 relativeVelocity = playerRigidbody.velocity - rb.velocity;

            if (relativeVelocity.magnitude > 1.0f)
            {
                Vector2 force = relativeVelocity * 10.0f;
                rb.AddForce(-force);
                playerRigidbody.AddForce(force);
            }



        }

    }


    public void Enemydead(GameObject obj)
    {
        enemyMove = false;
        enemydead = true;
        CircleCollider2D[] colliders = obj.GetComponents<CircleCollider2D>();

        foreach (CircleCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
        Destroy(obj, 10.0f);
        //        PointSystem.Instance.AddPoint(1);
    }

    public void Enemyattack()
    {
        if (!isEnemyAttackSoundPlaying)
        {
            audioSource.PlayOneShot(EnemyattacksoundClip);
            isEnemyAttackSoundPlaying = true;
            StartCoroutine(ResetSoundFlags());
        }


        if (Vector2.Distance(transform.position, warriorStatus.transform.position) <= attackRange)
        {
            if (!isPlayerHitSoundPlaying)
            {
                audioSource.PlayOneShot(PlayerHitsoundClip);
                isPlayerHitSoundPlaying = true;
                StartCoroutine(ResetSoundFlags());
            }
            warriorStatus.TakeDamagePlayer(10);
        }
    }

    IEnumerator ResetSoundFlags()
    {
        yield return new WaitForSeconds(0.1f); // ������ �����̸� �����մϴ�. ���⼭�� 0.1�ʸ� ����Ͽ����ϴ�.

        isEnemyAttackSoundPlaying = false;
        isPlayerHitSoundPlaying = false;

    }


    public void Enemyreaction()
    {
        animator.SetBool("isEnemyHit", false);
    }


}
