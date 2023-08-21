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
    public int currentDashStack;

    private int AttackCount;
    private bool isAttack;
    private float lastAttackTime;
    private float attackInterval = 0.5f;


    //대쉬
    public bool isCooldownRunning = false;
    
    public bool canUseSkillQ = true; //
    public bool canUseSkillE = true; //
    public static bool SkillEinvincibility = false;
    private static bool SkillEinvincibility2 = false;
    public static bool SkillQBoost = false;
    public static bool SkillEBoost = false;
    private static bool QBoostStart = false;
    private static bool EBoostStart = false;

    public AudioClip WarriorattacksoundClip; // Warrior 공격 사운드 클립
    public AudioClip WarriorSecondattacksoundClip; // Warrior 두번째공격 사운드 클립
    public AudioClip EnemyHitsoundClip; // 적 피격 사운드 클립
    public AudioClip BoxHitClip; // 상자피격 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private Vector3 mousePos;
    private Vector3 worldPos;
    private Vector3 direction;
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
    private bool usingSkillE = false; 
    private bool EskillMoveOff = false;
    private bool usingSkillQ = false;
    private bool isBasicAttacking = false;
    public void Awake()    //  시작되면서 스테이터스,rigidbody,애니메이터 컴포넌트 호출
    {
        stats = GetComponent<WarriorStatus>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // 게임 오브젝트에 AudioSource 컴포넌트를 추가
        audioSource = GetComponent<AudioSource>();
        currentDashStack = stats.dashStack;
       }
    public void Update()
    {
        if (GameManager.isPaused == false)
        {
            if (usingSkillQ)
            {
                rb.velocity = Vector2.zero;
            }
            UseSkillE();
            UseSkillQ();
            if (usingSkillE == false && usingSkillQ == false)
            {
                Move();
            }
            
            
            Attack();
            if (stats.deadCount)
            {
                rb.velocity = Vector2.zero;
            }
            if (currentDashStack < stats.dashStack && !isCooldownRunning)
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
            

            if (dashInput && canDash && currentDashStack > 0)
            {

                isDashing = true;
                if (currentDashStack == 0)
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
            if (Input.GetMouseButtonDown(0) && usingSkillE == false && usingSkillQ == false)    // 마우스 중복입력방지
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
            if (isAttack )
            {
                if (Input.GetMouseButtonDown(0) && usingSkillE == false && usingSkillQ == false)
                {
                    if (isBasicAttacking == false)
                    {
                        mousePos = Input.mousePosition;
                        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                        direction = (worldPos - transform.position).normalized;
                        Debug.Log(direction);

                    }
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
                if (usingSkillE)
                {
                    mousePos = Input.mousePosition;
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    direction = (worldPos - transform.position).normalized;
                    float absXD = Mathf.Abs(direction.x);
                    float absYD = Mathf.Abs(direction.y);
                    if (absXD > 0.2f && absYD > 0.2f) // 대각선 공격 좌표 계산
                    {
                        direction = new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y));
                    }
                    else if (absXD > absYD)
                    {
                        direction = new Vector2(Mathf.Sign(direction.x), 0);
                    }
                    else
                    {
                        direction = new Vector2(0, Mathf.Sign(direction.y));
                    }
                    Debug.Log("스킬 사용중");
                    if (EskillMoveOff == false)
                    {
                        Debug.Log("이동 작동");
                        rb.velocity = direction * stats.AttackMove;
                    }
                    else if(EskillMoveOff == true)
                    {
                        Debug.Log("이동 멈춤");
                        rb.velocity = Vector2.zero;
                    }
                }
                if (usingSkillE == false && usingSkillQ == false)
                {
                    if (AttackCount == 1)                           // 첫번째 공격 + 이동
                    {
                        rb.velocity = direction * stats.AttackMove;
                        anim.SetFloat("MovementX", rb.velocity.x);
                        anim.SetFloat("MovementY", rb.velocity.y);
                        FAtk.position = new Vector3(rb.position.x + direction.x, rb.position.y + direction.y, 0);
                    }
                    else if (AttackCount == 2)                      //두번째 공격
                    {
                        rb.velocity = direction * stats.AttackMove ;
                        anim.SetFloat("MovementX", rb.velocity.x);
                        anim.SetFloat("MovementY", rb.velocity.y);
                        SAtk.position = new Vector3(rb.position.x + direction.x, rb.position.y + direction.y, 0);
                    }
                }
            }
            
        }
    }

    private void UseSkillQ()
    {
        if (Input.GetKey(KeySetting.keys[KeyAction.Skill1]))
        {
            isAttack = true;
        }
        if (isAttack && canUseSkillQ)
        {
            if (Input.GetKey(KeySetting.keys[KeyAction.Skill1]))
            {
                
                usingSkillQ = true;
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

                
                anim.SetFloat("MovementX", direction.x);
                anim.SetFloat("MovementY", direction.y);
                if (SkillQBoost)
                {
                    QBoostStart = true;
                    if (QBoostStart)
                    {
                        anim.speed = 2.0f;
                    }

                }
                anim.SetBool("SkillQ", usingSkillQ);
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
        if (isAttack && canUseSkillE)
        {
            if (Input.GetKey(KeySetting.keys[KeyAction.Skill2]))
            {
                usingSkillE = true;
                if (SkillEinvincibility == true)
                {
                    SkillEinvincibility2 = true;
                    if (SkillEinvincibility2 == true)
                    {
                        WarriorStatus.isInvincible = true;
                    }
                }
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
                anim.SetFloat("MovementX", rb.velocity.x);
                anim.SetFloat("MovementY", rb.velocity.y);
                if (SkillEBoost)
                {
                    EBoostStart = true;
                    if (EBoostStart)
                    {
                        anim.speed = 2.0f;
                    }

                }             
                anim.SetBool("SkillE", usingSkillE);
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
        if (currentDashStack > 0)
        {
            currentDashStack--; // 대쉬 충전 감소
        }
    }
    private IEnumerator DashCoolDownC()   // 대쉬 쿨타임 돌리기 위한 코루틴
    {
        isCooldownRunning = true;
        yield return new WaitForSeconds(stats.DashCoolDown);         //대쉬 쿨다운만큼만큼 대기        
        if (currentDashStack < stats.dashStack)
        {
            currentDashStack++;
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
    }
    private void OffSecondAttack()
    {

    }
    private void QSkillAnimOff()
    {
        anim.speed = 1.0f;
        QBoostStart = false;
        usingSkillQ = false;
        anim.SetBool("SkillQ", usingSkillQ);
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
        anim.speed = 1.0f;
        EBoostStart = false;
        SkillEinvincibility2 = false;
        WarriorStatus.isInvincible = false;
        usingSkillE = false;
        anim.SetBool("SkillE", usingSkillE);
    }
    private void OnESkillAttack()
    {
        EskillMoveOff = true;
        ESkillATKM.isAttacking = true;
    }
    private void OffESkillAttackMove()
    {
    }
    private void ESkillAnimOn()
    {
        EskillMoveOff = false;
    }
    private void BasicAttackingOn()
    {
        Debug.Log("켜짐");
        isBasicAttacking = true;
    }
    private void BasicAttackingOff()
    {
        Debug.Log("꺼짐");
        isBasicAttacking = false;
    }
    private IEnumerator SkillCooldownQ()
    {
        canUseSkillQ = false;
        yield return new WaitForSeconds(stats.QCoolDown);
        canUseSkillQ = true;
    }  
    private IEnumerator SkillCooldownE()
    {
        canUseSkillE = false;
        yield return new WaitForSeconds(stats.ECoolDown);
        canUseSkillE = true;
    }

    private void atkMoveStop()
    {
        rb.velocity = Vector2.zero;
    }

}