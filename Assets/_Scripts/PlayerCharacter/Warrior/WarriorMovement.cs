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

    //대쉬
    public int dashCharges = 2;  // 대쉬 충전 횟수
    private bool isCooldownRunning = false;
    //

    public GameObject skillPrefabQ; // Q 스킬 프리팹
    public GameObject skillPrefabE; // E 스킬 프리팹
    private Collider2D skillColliderQ;
    private Collider2D skillColliderE;
    private bool isUsingSkill = false; // 스킬 사용 중인지를 나타내는 변수


    private bool canUseSkillQ = true; //
    private bool canUseSkillE = true; //


    private int skillQDamage = 30;
    private int skillEDamage = 40;

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
        skillColliderQ = skillPrefabQ.GetComponent<Collider2D>(); // skillPrefabQ의 Collider2D 컴포넌트 가져오기
        skillColliderE = skillPrefabE.GetComponent<Collider2D>();   /////// 

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
            if (!isUsingSkill) // 스킬을 사용 중이 아닐 때만 스킬 입력을 받음
            {
                if (Input.GetKey(KeySetting.keys[KeyAction.Skill1]))
                {
                    UseSkillQ();
                }
                else if (Input.GetKey(KeySetting.keys[KeyAction.Skill2]))
                {
                    UseSkillE();
                }

            }

            if (dashCharges < 2 && !isCooldownRunning)
            {
                StartCoroutine(DashCoolDownC());
            }
        }


    }
    public void Move() //  이동,구르기 애니메이션과 움직임 처리하는 함수
    {
        if (stats.deadCount == false)
        {
            float Horizontal = 0;
            float Vertical = 0;
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

            if (dashInput && canDash && dashCharges > 0)
            {
                
                isDashing = true;
                Debug.Log("대쉬 충전 수치: " + dashCharges); // 대쉬 충전 수치 디버그 출력
                if (dashCharges == 0)
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
        if (dashCharges > 0)
          {
            dashCharges--; // 대쉬 충전 감소
          }

    }

    private IEnumerator DashCoolDownC()   // 대쉬 쿨타임 돌리기 위한 코루틴
    {
        isCooldownRunning = true;
        yield return new WaitForSeconds(stats.DashCoolDown);         //대쉬 쿨다운만큼만큼 대기        
        if (dashCharges < 2)
        {
            dashCharges++;
            Debug.Log("대쉬 충전 수치: " + dashCharges); // 대쉬 충전 수치 디버그 출력    
            canDash = true; // 대쉬 쿨다운이 끝났으므로 다시 대쉬 가능하도록 설정
        }
        isCooldownRunning = false;
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

    private void UseSkillQ()
    {
        float distance = 3f;
        isUsingSkill = true; // 스킬 사용 중 플래그 설정
        CircleCollider2D skillColliderQ = skillPrefabQ.GetComponent<CircleCollider2D>();
        if (stats.deadCount == false && canUseSkillQ)
        {

            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 playerPosition = transform.position;
            Vector3 direction = (worldPos - playerPosition).normalized;
            Vector2 skillPosition = playerPosition + (direction * distance);
            Quaternion skillRotation = Quaternion.identity;

            float maxSkillDistance = 1f; // 스킬의 최대 거리
            float skillDistance = Vector2.Distance(skillPosition, playerPosition);
            if (skillDistance > maxSkillDistance)
            {
                skillPosition = playerPosition + (direction * maxSkillDistance);
            }

            rb.velocity = direction * stats.AttackMove;
            anim.SetFloat("MovementX", rb.velocity.x);
            anim.SetFloat("MovementY", rb.velocity.y);
            anim.SetBool("SkillQ", true);

            if (skillColliderQ != null)
            {
                float skillRangeQ = skillColliderQ.radius * 2f;


                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(skillPosition, skillRangeQ, enemyLayers);

                foreach (Collider2D enemy in hitEnemies)
                {
                    if (enemy != null)
                    {
                        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
                        if (enemyStatus != null)
                        {
                            // 적에게 데미지를 주는 처리를 수행합니다.
                            enemyStatus.TakeDamage(skillQDamage + stats.attackAddness);
                            Debug.Log("적이름" + enemy.name + "받은데미지" + (stats.power + stats.attackAddness) + "현재 체력" + enemyStatus.currentHealth);
                            audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior 공격 사운드
                        }
                    }
                }
            }
            StartCoroutine(SpawnSkillQAfterAnimation(skillPosition, skillRotation));
            StartCoroutine(SkillCooldownQ());

        }
        isUsingSkill = false; // 스킬 사용 종료 후 플래그 해제
    }




    private IEnumerator SpawnSkillQAfterAnimation(Vector2 skillPosition, Quaternion skillRotation)
    {

        yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기하여 애니메이션이 업데이트되도록 함

        // 스킬 프리팹을 인스턴스화하여 스킬 오브젝트 생성
        GameObject skillInstance = Instantiate(skillPrefabQ, skillPosition, skillRotation);

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 스킬 애니메이션 종료
        anim.SetBool("SkillQ", false);

        yield return new WaitForSeconds(1f); // 1초 대기

        // 스킬 프리팹 제거
        Destroy(skillInstance);
    }

    private IEnumerator SkillCooldownQ()
    {
        canUseSkillQ = false;
        float cooldownTimeQ = 1f; // Q 스킬의 쿨다운 시간(초)
        yield return new WaitForSeconds(cooldownTimeQ);
        canUseSkillQ = true;
    }


    private void UseSkillE()
    {
        float distance = 2f;
        isUsingSkill = true; // 스킬 사용 중 플래그 설정
        BoxCollider2D skillColliderE = skillPrefabE.GetComponent<BoxCollider2D>();
        if (stats.deadCount == false && canUseSkillE)
        {

            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 playerPosition = transform.position;
            Vector3 direction = (worldPos - playerPosition).normalized;
            Vector2 skillPosition = playerPosition + (direction * distance);
            Quaternion skillRotation = Quaternion.identity;

            float maxSkillDistance = 1f; // 스킬의 최대 거리
            float skillDistance = Vector2.Distance(skillPosition, playerPosition);
            if (skillDistance > maxSkillDistance)
            {
                skillPosition = playerPosition + (direction * maxSkillDistance);
            }

            rb.velocity = direction * stats.AttackMove;
            anim.SetFloat("MovementX", rb.velocity.x);
            anim.SetFloat("MovementY", rb.velocity.y);
            anim.SetBool("SkillE", true);

            if (skillColliderE != null)
            {
                Vector2 skillSizeE = skillColliderE.size;
                float skillRangeE = Mathf.Max(skillSizeE.x, skillSizeE.y);
                // 이후에 skillRangeE를 사용할 수 있습니다.
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(skillPosition, skillRangeE, enemyLayers);


                foreach (Collider2D enemy in hitEnemies)
                {
                    if (enemy != null)
                    {
                        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
                        if (enemyStatus != null)
                        {
                            // 적에게 데미지를 주는 처리를 수행합니다.

                            enemyStatus.TakeDamage(skillEDamage + stats.attackAddness);
                            Debug.Log("적이름" + enemy.name + "받은데미지" + (stats.power + stats.attackAddness) + "현재 체력" + enemyStatus.currentHealth);
                            audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior 공격 사운드
                        }
                    }


                }
            }
            StartCoroutine(SpawnSkillEAfterAnimation(skillPosition, skillRotation));
            StartCoroutine(SkillCooldownE());
        }
        isUsingSkill = false; // 스킬 사용 종료 후 플래그 해제
    }

    private IEnumerator SpawnSkillEAfterAnimation(Vector2 skillPosition, Quaternion skillRotation)
    {


        yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기하여 애니메이션이 업데이트되도록 함
                                              // 스킬 애니메이션 종료
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        anim.SetBool("SkillE", false);

        GameObject skillInstance = Instantiate(skillPrefabE, skillPosition, skillRotation);



        yield return new WaitForSeconds(1f); // 1초 대기

        // 스킬 프리팹 제거
        Destroy(skillInstance);
    }

    private IEnumerator SkillCooldownE()
    {
        canUseSkillE = false;
        float cooldownTimeE = 2f; // E 스킬의 쿨다운 시간(초)
        yield return new WaitForSeconds(cooldownTimeE);
        canUseSkillE = true;
    }

}