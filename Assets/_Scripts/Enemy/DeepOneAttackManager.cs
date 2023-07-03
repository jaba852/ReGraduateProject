using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class DeepOneAttackManager : MonoBehaviour
{
    private Animator animator;
    public bool enemyMove = true;
    public bool attackStop = false;
    public bool enemydead = false;
    public int count = 1;
    private Rigidbody2D rb;
    private WarriorStatus warriorStatus;
    private DeepOneMovement DeeponeMvmt;


    public float attackRange = 2.0f; // 일정 거리



    public GameObject warningPrefab;    // 경고 표시용 프리팹
    private Transform playerTransform;   // 플레이어 트랜스폼
    public float warningmoveSpeed = 5f;        // 경고 표시 이동 속도

    public SpriteRenderer energyPointSpriteRenderer;   // 투사체 스프라이트



    private GameObject warning;         // 경고 표시 오브젝트
    private Vector2 targetPosition;     // 플레이어 캐릭터 위치
    private bool isWarningActive = false; // 경고 표시 활성화 여부
    private bool isInAttackState = false; // DeepOne이 때린 공격인지 여부

    private bool DeepOneattacking = true; // DeepOne 침 뱉는중

    public AudioClip EnemyattacksoundClip; // Enemy 공격 사운드 클립
    public AudioClip PlayerHitsoundClip; // 피격 사운드 클립
    public AudioClip DeepOnesoundClip; // 딥원 공격 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private bool isEnemyAttackSoundPlaying = false; // Enemy 공격 사운드 재생 여부를 저장하는 변수
    private bool isPlayerHitSoundPlaying = false; // 피격 사운드 재생 여부를 저장하는 변수
    private bool isDeepOneAttackSoundPlaying = false; // 딥원 공격 사운드 재생 여부를 저장하는 변수




    public void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        warriorStatus = FindObjectOfType<WarriorStatus>();
        DeeponeMvmt = GetComponent<DeepOneMovement>();
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



    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!");
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 relativeVelocity = playerRigidbody.velocity - rb.velocity;

            if (relativeVelocity.magnitude > 2.0f)
            {
                Vector2 force = relativeVelocity * 10.0f;
                rb.AddForce(-force);
                playerRigidbody.AddForce(force);
            }

            if (isInAttackState) // DeepOne 투사체에 플레이어 캐릭터가 맞았을때
            {
                warriorStatus.TakeDamagePlayer(10);
                UnityEngine.Debug.Log("DeepOne 데미지");
                isInAttackState = false;
            }

        }

    }





    IEnumerator ResetSoundFlags()
    {
        yield return new WaitForSeconds(0.1f); // 적절한 딜레이를 설정합니다. 여기서는 0.1초를 사용하였습니다.

        isEnemyAttackSoundPlaying = false;
        isPlayerHitSoundPlaying = false;
        isDeepOneAttackSoundPlaying = false;

    }


    public void Enemyreaction()
    {
        animator.SetBool("isEnemyHit", false);
    }


    public void DeepOneattack()
    {
        if (!isWarningActive)
        {
            isWarningActive = true;

            if (warningPrefab != null) // warningPrefab이 null인지 확인
            {

                // 경고 표시 생성 및 초기화
                warning = Instantiate(warningPrefab, transform.position, Quaternion.identity);
                targetPosition = playerTransform.position;
                StartCoroutine(MoveAndAttack());
            }
        }
    }

    private IEnumerator MoveAndAttack()
    {
        float elapsedTime = 0f;
        DeepOneattacking = true;


        while (DeepOneattacking)
        {
            // 경고 표시를 플레이어 위치로 이동
            warning.transform.position = Vector2.MoveTowards(warning.transform.position, targetPosition, warningmoveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            // 2초 후에 탈출
            if (elapsedTime >= 1f)
            {
                break;
            }
            yield return null;
        }

        // 경고 표시 삭제
        Destroy(warning);
        isWarningActive = false;

        // energyPointSpriteRenderer 소환 및 제어
        Vector2 spawnPosition = targetPosition + new Vector2(0f, 1.5f);
        GameObject energyPoint = Instantiate(energyPointSpriteRenderer.gameObject, spawnPosition, Quaternion.identity);
        // energyPoint의 재생 속도 조절 (0.5는 임의의 값)
        energyPoint.GetComponent<Animator>().speed = 2f;
        StartCoroutine(MoveAndDestroyEnergyPoint(energyPoint, spawnPosition));
    }

    private IEnumerator MoveAndDestroyEnergyPoint(GameObject energyPoint, Vector2 spawnPosition)
    {
        // energyPoint를 spawnPosition 위치에 소환
        energyPoint.transform.position = spawnPosition;

        // energyPoint의 Animator 컴포넌트 가져오기
        Animator energyPointAnimator = energyPoint.GetComponent<Animator>();


        // energyPoint 애니메이션의 재생 시간 가져오기 - 뭔가 값이 안들어옴
        // float animationDuration = energyPointAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        StartCoroutine(PlayDeepOneshotSound());


        // energyPoint 애니메이션이 모두 재생될 때까지 기다림- 값이 안들어와서 animationDuration대신 값을 일일히 넣어줘야함
        yield return new WaitForSeconds(3f);

        // energyPoint 삭제
        Destroy(energyPoint);


    }

    IEnumerator PlayDeepOneshotSound()
    {
        yield return new WaitForSeconds(1.5f); // 사운드 재생 시작을 f초로 지연합니다.
        if (!isDeepOneAttackSoundPlaying)
        {
            audioSource.PlayOneShot(DeepOnesoundClip);
            yield return new WaitForSeconds(1.5f); // 사운드가 f초 동안 실행되도록 지연합니다.
            audioSource.Stop(); // 사운드 재생을 중지합니다.
            isDeepOneAttackSoundPlaying = true;
            StartCoroutine(ResetSoundFlags());
        }
    }



    public void DeepOneshot()
    {
        DeepOneattacking = false;
        Destroy(warning);
    }






}
