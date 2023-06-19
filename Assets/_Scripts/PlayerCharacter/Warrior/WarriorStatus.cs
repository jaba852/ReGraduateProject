using System.Collections;
using UnityEngine;

public class WarriorStatus : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerRenderer;

    [Header("�ǰ� ������ �ð�")]
    [SerializeField] public float blinkTime = 1f;

    [Header("�ִ�ü��")]
    [SerializeField] public int maxHealth = 100;

    [Header("����ü��")]
    [SerializeField] public int currentHealth;

    [Header("�̵��ӵ�")]
    [SerializeField] public float movementSpeed = 5f;

    [Header("�뽬 ���ӵ�")]
    [SerializeField] public float dashVelocity = 5f;

    [Header("�뽬 ���ӽð�")]
    [SerializeField] public float dashingTime = 0.9f;

    [Header("���� ���� ����")]
    [SerializeField] public float atkRangeRatio = 1f;

    [Header("���� �̵��Ÿ�")]
    [SerializeField] public float AttackMove = 2f;

    [Header("���� �ӵ�")]
    [SerializeField] public float atkSpeed = 1f;

    [Header("���ݷ�")]
    [SerializeField] public int power = 10;

    [Header("�߰�������")]
    [SerializeField] public int attackAddness = 1;

    [Header("�ǰݹ����ð�")]
    [SerializeField] public float invincibleDuration = 1f;

    [Header("������ ��Ÿ��")]
    [SerializeField] public float DashCoolDown = 3f;

    public bool deadCount;

    private bool isInvincible = false; // �ǰ� ���� ���¸� ��Ÿ���� ����

    private void Start()
    {
        // ���� �ʱ�ȭ
        LoadPlayerStats();

        deadCount = false;
    }
    private void OnDestroy()
    {
        // ���� ���� �Ǵ� �� ��ȯ �� ���� ����
        SavePlayerStats();
    }
    private void LoadPlayerStats()
    {
        currentHealth = maxHealth;
        // ���� ������ PlayerPrefs���� �ҷ���
        maxHealth = PlayerPrefs.GetInt("Player_MaxHealth", maxHealth);
        currentHealth = PlayerPrefs.GetInt("Current_Health", currentHealth);
        movementSpeed = PlayerPrefs.GetFloat("Movement_Speed", movementSpeed);
        dashVelocity = PlayerPrefs.GetFloat("Dash_Velocity", dashVelocity);
        dashingTime = PlayerPrefs.GetFloat("DashingTime", dashingTime);
        atkRangeRatio = PlayerPrefs.GetFloat("AtkRange_Ratio", atkRangeRatio);
        AttackMove = PlayerPrefs.GetFloat("Attack_Move", AttackMove);
        atkRangeRatio = PlayerPrefs.GetFloat("Attack_Speed", atkRangeRatio);
        power = PlayerPrefs.GetInt("Power", power);
        attackAddness = PlayerPrefs.GetInt("Attack_Addness", attackAddness);
        invincibleDuration = PlayerPrefs.GetFloat("InvincibleDuration", invincibleDuration);
        DashCoolDown = PlayerPrefs.GetFloat("DashCoolDown", DashCoolDown);


    }
    private void SavePlayerStats()
    {
        // ���� ���� PlayerPrefs�� ����
        PlayerPrefs.SetInt("Player_MaxHealth", maxHealth);
        PlayerPrefs.SetInt("Current_Health", currentHealth);
        PlayerPrefs.SetFloat("Movement_Speed", movementSpeed);
        PlayerPrefs.SetFloat("Dash_Velocity", dashVelocity);
        PlayerPrefs.SetFloat("DashingTime", dashingTime);
        PlayerPrefs.SetFloat("AtkRange_Ratio", atkRangeRatio);
        PlayerPrefs.SetFloat("Attack_Move", AttackMove);
        PlayerPrefs.SetFloat("Attack_Speed", atkRangeRatio);
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetInt("Attack_Addness", attackAddness);
        PlayerPrefs.SetFloat("InvincibleDuration", invincibleDuration);
        PlayerPrefs.SetFloat("DashCoolDown", DashCoolDown);
        // PlayerPrefs ���� ������ ����
        PlayerPrefs.Save();
    }

    public void TakeDamagePlayer(int damage)
    {
        if (isInvincible || deadCount || WarriorMovement.isDashing)
        {
            return; // �ǰݹ���, ���, �뽬 �߿� ����
        }

        currentHealth -= damage;
        Debug.Log("�÷��̾� �ǰ�" + damage);

        if (currentHealth <= 0)
        {
            deadCount = true;
            GetComponent<Animator>().SetBool("isDead", deadCount);
            InitializePlayerStats(); // ���� �ʱ�ȭ
        }
        else
        {
            StartCoroutine(PlayerHit());
            StartCoroutine(invincibility());
        }
    }

    private IEnumerator PlayerHit()
    {
        if (deadCount)
        {
            yield return null;
        }
        float startTime = Time.time;
        while (Time.time - startTime < blinkTime)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            playerRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        playerRenderer.enabled = true; // �ǰ� ���� �� �÷��̾ �ٽ� Ȱ��ȭ�մϴ�.


    }
    private IEnumerator invincibility()
    {
        isInvincible = true; // �ǰ� ���� ���� Ȱ��ȭ

        // �ǰ� ȿ��, �ִϸ��̼� �� �߰� ó��

        yield return new WaitForSeconds(invincibleDuration); // �ǰ� ���� ���� �ð���ŭ ���

        isInvincible = false; // �ǰ� ���� ���� ��Ȱ��ȭ
    }

    public void InitializePlayerStats()
    {
        
        maxHealth = 100;
        
        movementSpeed = 5f;
        
        dashVelocity = 5f;

        dashingTime = 0.9f;

        atkRangeRatio = 1f;

        AttackMove = 2f;

        atkSpeed = 1f;
        
        power = 10;

        attackAddness = 1;

        invincibleDuration = 1f;

        DashCoolDown = 3f;

        // �÷��̾��� ü���� �ʱ�ȭ�� ������ ����
        currentHealth = maxHealth;

        // CircleCollider2D ��Ȱ��ȭ
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("CircleCollider2D ������Ʈ�� ã�� �� �����ϴ�!");
        }

        // Rigidbody2D ��Ȱ��ȭ
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.simulated = false;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

}
