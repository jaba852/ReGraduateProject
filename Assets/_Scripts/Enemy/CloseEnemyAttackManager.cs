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



    public float attackRange = 2.0f; // 일정 거리

    private Transform playerTransform;   // 플레이어 트랜스폼

    private Vector2 targetPosition;     // 플레이어 캐릭터 위치

    public AudioClip EnemyattacksoundClip; // Enemy 공격 사운드 클립
    public AudioClip PlayerHitsoundClip; // 피격 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private bool isEnemyAttackSoundPlaying = false; // Enemy 공격 사운드 재생 여부를 저장하는 변수
    private bool isPlayerHitSoundPlaying = false; // 피격 사운드 재생 여부를 저장하는 변수





    public void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        warriorStatus = FindObjectOfType<WarriorStatus>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform; // 찾은 게임 오브젝트의 Transform을 가져옵니다.
        }

        // 게임 오브젝트에 AudioSource 컴포넌트를 추가
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
        UnityEngine.Debug.Log("Warrior를 발견!");
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
            UnityEngine.Debug.Log("count 초기화");
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
        yield return new WaitForSeconds(0.1f); // 적절한 딜레이를 설정합니다. 여기서는 0.1초를 사용하였습니다.

        isEnemyAttackSoundPlaying = false;
        isPlayerHitSoundPlaying = false;

    }


    public void Enemyreaction()
    {
        animator.SetBool("isEnemyHit", false);
    }


}
