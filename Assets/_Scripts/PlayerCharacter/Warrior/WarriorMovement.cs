using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    public WarriorStatus stats;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 MovementInput;
    public LayerMask enemyLayers;

    private Vector2 dashingDir;
    public static bool isDashing;
    public bool canDash = true;

    private int AttackCount;
    private bool isAttack;
    private float lastAttackTime;
    private float attackInterval = 0.5f;


    //대쉬
    public bool isCooldownRunning = false;
    //

    public bool canUseSkillQ = true; //
    public bool canUseSkillE = true; //

    public float skillQCooldownTime = 3f; // Q 스킬의 쿨다운 시간(초)
    public float skillECooldownTime = 5f; // E 스킬의 쿨다운 시간(초)

    public static bool isStunSkillLearned = false; // 스턴 스킬을 배웠는지 여부
    public float StunTime = 1.5f;

    public AudioClip WarriorattacksoundClip; // Warrior 공격 사운드 클립
    public AudioClip WarriorSecondattacksoundClip; // Warrior 두번째공격 사운드 클립
    public AudioClip EnemyHitsoundClip; // 적 피격 사운드 클립
    public AudioClip BoxHitClip; // 상자피격 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private Vector3 mousePos;
    private Vector3 worldPos;
    private Vector3 direction;
    private bool isIdle;

    public Transform FAtk;
    public Transform SAtk;
    public Transform QAtk;
    public Transform EAtk;
    public WarriorAttackManagement FirstATKM;
    public WarriorAttackManagement SecondATKM;
    public WarriorAttackManagement QSkillATKM;
    public WarriorAttackManagement ESkillATKM;

    private float Horizontal;
    private float Vertical;
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
            UseSkillE();
            UseSkillQ();
            Move();
            Attack();
            if (stats.deadCount)
            {
                rb.velocity = Vector2.zero;
            }
            if (stats.dashStack < 2 && !isCooldownRunning)
            {
                StartCoroutine(DashCoolDownC());
            }
        }
    }

    public void Move() //  이동,구르기 애니메이션과 움직임 처리하는 함수
    {
        if (stats.deadCount == false)
        {
            Horizontal = 0;
            Vertical = 0;
            bool dashInput = false;
            if (Input.GetKey(KeySetting.keys[KeyAction.Up]))
            {
                Vertical = 1;
            }
            if (Input.GetKey(KeySetting.keys[KeyAction.Down]))
            {
                Vertical = -1;

            }
            if (Input.GetKey(KeySetting.keys[KeyAction.Left]))
            {
                Horizontal = -1;
            }
            if (Input.GetKey(KeySetting.keys[KeyAction.Right]))
            {
                Horizontal = 1;
            }
            if (Input.GetKeyDown(KeySetting.keys[KeyAction.Dash]))
            {
                dashInput = true;
            }

            MovementInput = new Vector2(Horizontal, Vertical);
            Debug.Log("이동입력" + MovementInput);

            if (MovementInput.x != 0 || MovementInput.y != 0)
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
            else if (Horizontal == 0 && Vertical == 0)
            {
                rb.velocity = Vector2.zero;

            }
            if (rb.velocity == Vector2.zero)
            {
                anim.SetBool("isMoving", false);
            }
            
            if (AttackCount == 2)
            {
                rb.velocity = Vector2.zero;
            }

            if (dashInput && canDash && stats.dashStack > 0)
            {

                isDashing = true;
                Debug.Log("대쉬 충전 수치: " + stats.dashStack); // 대쉬 충전 수치 디버그 출력
                if (stats.dashStack == 0)
                {
                    canDash = false;
                }
                dashingDir = new Vector2(Horizontal, Vertical);

                StartCoroutine(StopDashing());

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
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
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
                if (Input.GetMouseButtonDown(0))
                {
                    mousePos = Input.mousePosition;
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    direction = (worldPos - transform.position).normalized;
                };

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
                    FAtk.position = new Vector3(rb.position.x + direction.x, rb.position.y + direction.y, 0);
                }
                else if (AttackCount == 2)                      //두번째 공격
                {
                    rb.velocity = direction * stats.AttackMove * 0.1f;
                    anim.SetFloat("MovementX", rb.velocity.x);
                    anim.SetFloat("MovementY", rb.velocity.y);
                    SAtk.position = new Vector3(rb.position.x + direction.x, rb.position.y + direction.y, 0);
                }
            }
            
        }
    }

    private void UseSkillQ()
    {
        Debug.Log("Q스킬눌림" + isAttack);
        if (Input.GetKey(KeySetting.keys[KeyAction.Skill1]))
        {
            isAttack = true;
        }
        if (isAttack && canUseSkillQ)
        {
            if (Input.GetKey(KeySetting.keys[KeyAction.Skill1]))
            {
                mousePos = Input.mousePosition;
                worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                direction = (worldPos - transform.position).normalized;
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

                Debug.Log("Q스킬 방향" + direction);
                anim.SetFloat("MovementX", direction.x);
                anim.SetFloat("MovementY", direction.y);
                anim.SetBool("SkillQ", true);
                QAtk.position = new Vector3(rb.position.x + direction.x, rb.position.y + direction.y, 0);
                StartCoroutine(SkillCooldownQ());
            }
        }
    }
    private void UseSkillE()
    {
        if (Input.GetKey(KeySetting.keys[KeyAction.Skill2]))
        {
            isAttack = true;
        }
            Debug.Log("E스킬눌림" + isAttack);
        if (isAttack && canUseSkillE)
        {
            if (Input.GetKey(KeySetting.keys[KeyAction.Skill2]))
            {
                mousePos = Input.mousePosition;
                worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                direction = (worldPos - transform.position).normalized;
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
                
                rb.velocity = direction * stats.AttackMove; // 여기서 움직임을 설정합니다.
                Debug.Log("E스킬 이동" + rb.velocity);
                anim.SetFloat("MovementX", direction.x);
                anim.SetFloat("MovementY", direction.y);
                anim.SetBool("SkillE", true);
                EAtk.position = new Vector3(rb.position.x + direction.x, rb.position.y + direction.y, 0);
                StartCoroutine(SkillCooldownE());
            }
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
        if (stats.dashStack > 0)
        {
            stats.dashStack--; // 대쉬 충전 감소
        }
    }
    private IEnumerator DashCoolDownC()   // 대쉬 쿨타임 돌리기 위한 코루틴
    {
        isCooldownRunning = true;
        yield return new WaitForSeconds(stats.DashCoolDown);         //대쉬 쿨다운만큼만큼 대기        
        if (stats.dashStack < 2)
        {
            stats.dashStack++;
            Debug.Log("대쉬 충전 수치: " + stats.dashStack); // 대쉬 충전 수치 디버그 출력    
            canDash = true; // 대쉬 쿨다운이 끝났으므로 다시 대쉬 가능하도록 설정
        }
        isCooldownRunning = false;
    }
    private void OnFirstAttack()
    {
        audioSource.PlayOneShot(WarriorattacksoundClip); // Warrior 공격 사운드
        FirstATKM.isAttacking = true;

    }
    private void OffFirstAttack()
    {

    }
    private void OnSecondAttack()
    {
        audioSource.PlayOneShot(WarriorSecondattacksoundClip); // Warrior 공격 사운드
        SecondATKM.isAttacking = true;
        Debug.Log(" 두번쨰공격");
    }
    private void OffSecondAttack()
    {

    }
    private void QSkillAnimOff()
    {
        anim.SetBool("SkillQ", false);
    }
    private void OnQSkillAttack()
    {
        QSkillATKM.isAttacking = true;
    }
    private void OffQSkillAttack()
    {
    }
    private void ESkillAnimOff()
    {
        anim.SetBool("SkillE", false);
    }
    private void OnESkillAttack()
    {
        rb.velocity = Vector2.zero;
        ESkillATKM.isAttacking = true;
    }
    private void OffESkillAttackMove()
    {
    }
    private IEnumerator SkillCooldownQ()
    {
        canUseSkillQ = false;
        yield return new WaitForSeconds(skillQCooldownTime);
        canUseSkillQ = true;
    }  
    private IEnumerator SkillCooldownE()
    {
        canUseSkillE = false;
        yield return new WaitForSeconds(skillECooldownTime);
        canUseSkillE = true;
    }

    private void atkMoveStop()
    {
        rb.velocity = Vector2.zero;
    }

}