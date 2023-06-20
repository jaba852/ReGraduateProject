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

    public AudioClip WarriorattacksoundClip; // Warrior ���� ���� Ŭ��
    public AudioClip WarriorSecondattacksoundClip; // Warrior �ι�°���� ���� Ŭ��
    public AudioClip EnemyHitsoundClip; // �� �ǰ� ���� Ŭ��
    public AudioClip BoxHitClip; // �����ǰ� ���� Ŭ��
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ


    public void Awake()    //  ���۵Ǹ鼭 �������ͽ�,rigidbody,�ִϸ����� ������Ʈ ȣ��
    {
        stats = GetComponent<WarriorStatus>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // ���� ������Ʈ�� AudioSource ������Ʈ�� �߰�
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
    public void Move() //  �̵�,������ �ִϸ��̼ǰ� ������ ó���ϴ� �Լ�
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
                if (Horizontal != 0 && Vertical != 0) // �밢�� �̵� �� �̵� �ӵ� ����
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
    private void Attack()   //  ���� �ִϸ��̼��� ó���ϴ� �Լ�
    {
        if (stats.deadCount == false)
        {
            attackInterval = attackInterval / stats.atkSpeed;
            anim.SetFloat("AttackSpeed", stats.atkSpeed); // ���ݼӵ������� �ִϸ��̼� �ӵ� ����
            if (Input.GetMouseButtonDown(0))    // ���콺 �ߺ��Է¹���
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
                if (absX > 0.2f && absY > 0.2f) // �밢�� ���� ��ǥ ���
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

                if (AttackCount == 1)                           // ù��° ���� + �̵�
                {
                    rb.velocity = direction * stats.AttackMove;
                    anim.SetFloat("MovementX", rb.velocity.x);
                    anim.SetFloat("MovementY", rb.velocity.y);
                    StartCoroutine(FirstAttackBdelay(direction));
                }

                else if (AttackCount == 2)                      //�ι�° ����
                {
                    StartCoroutine(SecondAttackBdelay(direction));
                }
            }
        }
    }
    private void OnDrawGizmos() // ���ݹ��� gizmo�� �׸���
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
    public void CheckAttackPhase()  // ���� �ܰ踦 Ȯ���ϱ� ���� �Լ�
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
    private void ResetAttackCount() // ���� �ܰ踦 �ʱ��ϱ� ���� �Լ�
    {
        AttackCount = 0;
        anim.SetInteger("AttackCount", AttackCount);
    }
    private IEnumerator StopDashing()   // �뽬 ��Ÿ�� ������ ���� �ڷ�ƾ
    {
        if (stats.deadCount)
        {
            yield return null;
        }
        yield return new WaitForSeconds(stats.dashingTime);         //�뽬 �ð���ŭ ���
        isDashing = false;
        anim.SetBool("isRolling", false);                           //�뽬 �ִϸ��̼� ��
    }
    private IEnumerator FirstAttackBdelay(Vector2 direction)    // ù��° �������� �ι� �����ؼ� �߰� �������� ���������� ȿ�����
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
            audioSource.PlayOneShot(WarriorattacksoundClip);// Warrior ���� ����
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
                        Debug.Log("���̸�" + enemy.name + "����������" + (stats.power + stats.attackAddness) + "���� ü��" + enemyStatus.currentHealth);
                        audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior ���� ����
                        yield return new WaitForSeconds(0.2f / stats.atkSpeed);
                        enemyStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("���̸�" + enemy.name + "����������" + (stats.power + stats.attackAddness) + "���� ü��" + enemyStatus.currentHealth);
                        audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior ���� ����
                    }
                    DoorStatus doorStatus = enemy.GetComponent<DoorStatus>();
                    if (doorStatus != null)
                    {
                        doorStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("�� �̸�: " + enemy.name + ", ���� ������: " + (stats.power + stats.attackAddness) + ", ���� ü��: " + doorStatus.currentHealth);
                        audioSource.PlayOneShot(BoxHitClip);// Warrior ���� ����
                        yield return new WaitForSeconds(0.2f / stats.atkSpeed);
                        doorStatus.TakeDamage(stats.power + stats.attackAddness);
                        Debug.Log("�� �̸�: " + enemy.name + ", ���� ������: " + (stats.power + stats.attackAddness) + ", ���� ü��: " + doorStatus.currentHealth);
                        audioSource.PlayOneShot(BoxHitClip);// Warrior ���� ����
                    }
                }
            }
            StartCoroutine(isAttackingD());
        }
        yield break;

    }
    private IEnumerator SecondAttackBdelay(Vector2 direction)   // �ι�° �������� �����ݷ��� �������� ȿ�����
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
            audioSource.PlayOneShot(WarriorSecondattacksoundClip); // Warrior ���� ����
            isAttacking = false;
            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPointPosition, secondatattackRange * stats.atkRangeRatio, enemyLayers);
            foreach (Collider2D enemy in hitenemies)
            {
                EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
                if (enemyStatus != null)
                {
                    enemyStatus.TakeDamage(stats.power * SecondARatio + stats.attackAddness);
                    Debug.Log("���̸�" + enemy.name + "����������" + (stats.power * SecondARatio + stats.attackAddness) + "���� ü��" + enemyStatus.currentHealth);
                    audioSource.PlayOneShot(EnemyHitsoundClip);// Warrior ���� ����
                }
                DoorStatus doorStatus = enemy.GetComponent<DoorStatus>();
                if (doorStatus != null)
                {
                    doorStatus.TakeDamage(stats.power + stats.attackAddness);
                    Debug.Log("�� �̸�: " + enemy.name + ", ���� ������: " + (stats.power + stats.attackAddness) + ", ���� ü��: " + doorStatus.currentHealth);
                    audioSource.PlayOneShot(BoxHitClip);// Warrior ���� ����
                    yield return new WaitForSeconds(0.2f / stats.atkSpeed);
                    doorStatus.TakeDamage(stats.power + stats.attackAddness);
                    Debug.Log("�� �̸�: " + enemy.name + ", ���� ������: " + (stats.power + stats.attackAddness) + ", ���� ü��: " + doorStatus.currentHealth);
                    audioSource.PlayOneShot(BoxHitClip);// Warrior ���� ����
                }
            }
            StartCoroutine(isAttackingD());
        }

    }
    private IEnumerator isAttackingD()  // �ѹ��� ������ �����ϴ°� �����ϱ����� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f / stats.atkSpeed);
        isAttacking = true;
    }
    private IEnumerator DashCoolDownC()   // �뽬 ��Ÿ�� ������ ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(stats.DashCoolDown);         //�뽬 ��ٿŭ��ŭ ���
        canDash = true;                                             //�뽬 �����ϰ� Ȱ��ȭ
    }

}