using System.Collections;
using UnityEditor;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    public WarriorStatus stats;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 MovementInput;
    public Transform AttackPoint;
    public LayerMask enemyLayers;

    private Vector2 dashingDir;
    public static bool isDashing;
    public bool canDash = true;

    private int AttackCount;
    private bool isAttack;
    private float FAttackBDelay = 0.2f;
    private float SAttackBDelay = 0.5f;
    private float lastAttackTime;
    private float attackInterval = 0.5f;
    private float firstattackRange = 1f;
    private float secondatattackRange = 1.5f;
    private bool isAttacking = true;
    private float SecondARatio = 1.5f;

    public AudioClip WarriorattacksoundClip; // Warrior 공격 사운드 클립
    public AudioClip WarriorSecondattacksoundClip; // Warrior 두번째공격 사운드 클립
    public AudioClip EnemyHitsoundClip; // 적 피격 사운드 클립
    public AudioClip BoxHitClip; // 상자피격 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트


    public void Awake()    //  시작되면서 스테이터스,rigidbody,애니메이터 컴포넌트 호출
    {
        stats = GetComponent<WarriorStatus>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // 게임 오브젝트에 AudioSource 컴포넌트를 추가
        audioSource = GetComponent<AudioSource>();

    }
    public void Update()
    {
        if (GameManager.isPaused == false)
        {
            Move();

            Attack();

            if (stats.deadCount)
            {
                rb.velocity = Vector2.zero;
            }
        }

    }
    public void Move() //  이동,구르기 애니메이션과 움직임 처리하는 함수
    {
        if (stats.deadCount == false)
        {
            float Horizontal = Input.GetAxisRaw("Horizontal");
            float Vertical = Input.GetAxisRaw("Vertical");
            bool dashInput = Input.GetButtonDown("Dash");

            MovementInput = new Vector2(Horizontal, Vertical);

            bool isIdle = (Horizontal == 0 && Vertical == 0);
            if (isIdle)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("isMoving", false);

            }
            else if (MovementInput.x != 0 || MovementInput.y != 0)
            {
                float moveSpeed = stats.movementSpeed;
                if (Horizontal != 0 && Vertical != 0) // 대각선 이동 시 이동 속도 조정
                {
                    moveSpeed /= Mathf.Sqrt(2);
                }
                rb.velocity = MovementInput * moveSpeed;
                anim.SetFloat("MovementX", Horizontal);
                anim.SetFloat("MovementY", Vertical);
                anim.SetBool("isMoving", true);
            }
            if (AttackCount == 2)
            {
                rb.velocity = Vector2.zero;
            }

            if (dashInput && canDash)
            {
                isDashing = true;
                canDash = false;
                dashingDir = new Vector2(Horizontal, Vertical);
                StartCoroutine(StopDashing());
                StartCoroutine(DashCoolDownC());
            }

            if (isDashing)
            {
                rb.velocity = dashingDir.normalized * stats.dashVelocity;

                anim.SetFloat("MovementX", rb.velocity.x);
                anim.SetFloat("MovementY", rb.velocity.y);
                anim.SetBool("isRolling", true);

                return;
            }
        }
    }
    private void Attack()   //  공격 애니메이션을 처리하는 함수
    {
        if (stats.deadCount == false)
        {
            attackInterval = attackInterval / stats.atkSpeed;
            anim.SetFloat("AttackSpeed", stats.atkSpeed); // 공격속도에따라서 애니메이션 속도 조절
            if (Input.GetMouseButtonDown(0))    // 마우스 중복입력방지
            {
                if (Time.time - lastAttackTime >= attackInterval * stats.atkSpeed)
                {
                    isAttack = true;
                    AttackCount++;
                    if (AttackCount == 1)
                    {
                        anim.SetInteger("AttackCount", AttackCount);
                    }
                    lastAttackTime = Time.time;
                }

            }
            if (isAttack)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                Vector2 direction = (worldPos - transform.position).normalized;
                float absX = Mathf.Abs(direction.x);
                float absY = Mathf.Abs(direction.y);
                if (absX > 0.2f && absY > 0.2f) // 대각선 공격 좌표 계산
                {
                    direction = new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y));
                }
                else if (absX > absY)
                {
                    direction = new Vector2(Mathf.Sign(direction.x), 0);
                }
                else
                {
                    direction = new Vector2(0, Mathf.Sign(direction.y));
                }

                if (AttackCount == 1)                           // 첫번째 공격 + 이동
                {
                    rb.velocity = direction * stats.AttackMove;
                    anim.SetFloat("MovementX", rb.velocity.x);
                    anim.SetFloat("MovementY", rb.velocity.y);
                    StartCoroutine(FirstAttackBdelay(direction));
                }

                else if (AttackCount == 2)                      //두번째 공격
                {
                    StartCoroutine(SecondAttackBdelay(direction));
                }
            }
        }
    }
    private void OnDrawGizmos() // 공격범위 gizmo로 그리기
    {

        if (AttackPoint == null)
        {
            return;
        }
        if (AttackCount == 1)
        {
            Gizmos.DrawWireSphere(AttackPoint.position, firstattackRange * stats.atkRangeRatio);
        }
        if (AttackCount == 2)
        {
            Gizmos.DrawWireSphere(AttackPoint.position, secondatattackRange * stats.atkRangeRatio);

        }
    }
    public void CheckAttackPhase()  // 공격 단계를 확인하기 위한 함수
    {
        isAttack = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            if (AttackCount > 1)
            {
                anim.SetInteger("AttackCount", 2);
            }
            else
            {
                ResetAttackCount();

            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            if (AttackCount >= 2)
            {
                ResetAttackCount();
            }
        }
    }
    private void ResetAttackCount() // 공격 단계를 초기하기 위한 함수
    {
        AttackCount = 0;
        anim.SetInteger("AttackCount", AttackCount);
    }
    private IEnumerator StopDashing()   // 대쉬 쿨타임 돌리기 위한 코루틴
    {
        if (stats.deadCount)
        {
            yield return null;
        }
        yield return new WaitForSeconds(stats.dashingTime);         //대쉬 시간만큼 대기
        isDashing = false;
        anim.SetBool("isRolling", false);                           //대쉬 애니메이션 끝
    }
    private IEnumerator FirstAttackBdelay(Vector2 direction)    // 첫번째 공격으로 두번 공격해서 추가 데미지가 높아질수록 효율상승
    {
        if (stats.deadCount)
        {
            yield return null;
        }
        yield return new WaitForSeconds(FAttackBDelay / stats.atkSpeed);

        Vector2 attackPointPosition = rb.position + direction * 0.5f;
        AttackPoint.position = attackPointPosition;
        if (isAttacking)
        {
            audioSource.PlayOneShot(WarriorattacksoundClip);// Warrior 공격 사운드
            isAttacking = false;
            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPointPosition, firstattackRange * stats.atkRangeRatio, enemyLayers);
            foreach (Collider2D enemy in hitenemies)
            {
                if (enemy != null)
                {
                    EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
                    if (enemyStatus != null)
                    {
                        enemyStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("적이름" + enemy.name + "받은데미지" + (stats.power + stats.attackAddness) + "현재 체력" + enemyStatus.currentHealth);
                        audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior 공격 사운드
                        yield return new WaitForSeconds(0.2f / stats.atkSpeed);
                        enemyStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("적이름" + enemy.name + "받은데미지" + (stats.power + stats.attackAddness) + "현재 체력" + enemyStatus.currentHealth);
                        audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior 공격 사운드
                    }
                    DoorStatus doorStatus = enemy.GetComponent<DoorStatus>();
                    if (doorStatus != null)
                    {
                        doorStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("문 이름: " + enemy.name + ", 받은 데미지: " + (stats.power + stats.attackAddness) + ", 현재 체력: " + doorStatus.currentHealth);
                        audioSource.PlayOneShot(BoxHitClip);// Warrior 공격 사운드
                        yield return new WaitForSeconds(0.2f / stats.atkSpeed);
                        doorStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("문 이름: " + enemy.name + ", 받은 데미지: " + (stats.power + stats.attackAddness) + ", 현재 체력: " + doorStatus.currentHealth);
                        audioSource.PlayOneShot(BoxHitClip);// Warrior 공격 사운드
                    }
                }
            }
            StartCoroutine(isAttackingD());
        }
        yield break;

    }
    private IEnumerator SecondAttackBdelay(Vector2 direction)   // 두번째 공격으로 깡공격력이 높을수록 효율상승
    {
        if (stats.deadCount)
        {
            yield return null;
        }
        yield return new WaitForSeconds(SAttackBDelay / stats.atkSpeed);

        Vector2 attackPointPosition = rb.position + direction * 0.5f;
        AttackPoint.position = attackPointPosition;
        if (isAttacking)
        {
            audioSource.PlayOneShot(WarriorSecondattacksoundClip); // Warrior 공격 사운드
            isAttacking = false;
            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPointPosition, secondatattackRange * stats.atkRangeRatio, enemyLayers);
            foreach (Collider2D enemy in hitenemies)
            {
                EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
                if (enemyStatus != null)
                {
                    enemyStatus.TakeDamage(stats.power * SecondARatio + stats.attackAddness);
                    Debug.Log("적이름" + enemy.name + "받은데미지" + (stats.power * SecondARatio + stats.attackAddness) + "현재 체력" + enemyStatus.currentHealth);
                    audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior 공격 사운드
                }
                DoorStatus doorStatus = enemy.GetComponent<DoorStatus>();
                if (doorStatus != null)
                {
                    doorStatus.TakeDamage(stats.power + stats.attackAddness);
                    Debug.Log("문 이름: " + enemy.name + ", 받은 데미지: " + (stats.power + stats.attackAddness) + ", 현재 체력: " + doorStatus.currentHealth);
                    audioSource.PlayOneShot(BoxHitClip);// Warrior 공격 사운드
                    yield return new WaitForSeconds(0.2f / stats.atkSpeed);
                    doorStatus.TakeDamage(stats.power + stats.attackAddness);
                    Debug.Log("문 이름: " + enemy.name + ", 받은 데미지: " + (stats.power + stats.attackAddness) + ", 현재 체력: " + doorStatus.currentHealth);
                    audioSource.PlayOneShot(BoxHitClip);// Warrior 공격 사운드
                }
            }
            StartCoroutine(isAttackingD());
        }

    }
    private IEnumerator isAttackingD()  // 한번에 여러번 공격하는걸 방지하기위한 코루틴
    {
        yield return new WaitForSeconds(1f / stats.atkSpeed);
        isAttacking = true;
    }
    private IEnumerator DashCoolDownC()   // 대쉬 쿨타임 돌리기 위한 코루틴
    {
        yield return new WaitForSeconds(stats.DashCoolDown);         //대쉬 쿨다운만큼만큼 대기
        canDash = true;                                             //대쉬 가능하게 활성화
    }

}