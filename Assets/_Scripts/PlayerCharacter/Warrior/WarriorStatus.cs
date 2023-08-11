using System;
using System.Collections;
using UnityEngine;

public class WarriorStatus : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerRenderer;

    [Header("�÷��̾� ����")]
    [SerializeField] public int playerLevel = 1;

    [Header("�÷��̾� ���� ����ġ")]
    [SerializeField] public int playerExp = 0;

    [Header("�÷��̾� �䱸 ����ġ")]
    [SerializeField] public int maxExp = 10;

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

    [Header("�뽬 ����")]
    [SerializeField] public int dashStack = 1;

    [Header("�⺻���� ���� ����")]
    [SerializeField] public float atkRangeScale = 1f;

    [Header("�ι�° �⺻���� ���� ����")]
    [SerializeField] public float SecondAtkRangeScale = 1.5f;

    [Header("Q��ų ���� ���� ����")]
    [SerializeField] public float QSkillRangeScale = 0.8f;

    [Header("E��ų ���� ���� ����")]
    [SerializeField] public float ESkillRangeScale = 2f;

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
    [SerializeField] public int DashCoolDown = 10;

    [Header("Q��ų ��Ÿ��")]
    [SerializeField] public int QCoolDown = 10;

    [Header("E��ų ��Ÿ��")]
    [SerializeField] public int ECoolDown = 15;

    [Header("2Ÿ �⺻���� ����")]
    [SerializeField] public float SecondDamageScale = 2f;

    [Header("Q��ų ������ ���")]
    [SerializeField] public float QSkillDamageScale = 2f;

    [Header("E��ų ������ ���")]
    [SerializeField] public float ESkillDamageScale = 3f;


    [Header("���⼭���� ��ųƮ�� 0�̶�� �̽��� 1,2,3�̶�� �ش��ϴ°� ����")]
    [SerializeField] public int SkillTree1 = 0;
    [SerializeField] public int SkillTree3 = 0;
    [SerializeField] public int SkillTree5 = 0;
    [SerializeField] public int SkillTree7 = 0;
    [SerializeField] public int SkillTree10 = 0;
    [SerializeField] public int SkillTree13 = 0;
    [SerializeField] public int SkillTree15 = 0;
    [SerializeField] public int SkillTree18 = 0;
    [SerializeField] public int SkillTree20 = 0;

    [Header("�׾��°�")]
    public bool deadCount;


    public event Action HealthChanged;

    public event Action ExpChanged;

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
        playerLevel = PlayerPrefs.GetInt("playerLevel", playerLevel);
        playerExp = PlayerPrefs.GetInt("playerExp", playerExp);
        maxExp = PlayerPrefs.GetInt("maxExp", maxExp);
        maxHealth = PlayerPrefs.GetInt("Player_MaxHealth", maxHealth);
        currentHealth = PlayerPrefs.GetInt("Current_Health", currentHealth);
        movementSpeed = PlayerPrefs.GetFloat("Movement_Speed", movementSpeed);
        dashVelocity = PlayerPrefs.GetFloat("Dash_Velocity", dashVelocity);
        dashingTime = PlayerPrefs.GetFloat("DashingTime", dashingTime);
        dashStack = PlayerPrefs.GetInt("dashStack", dashStack);
        atkRangeScale = PlayerPrefs.GetFloat("AtkRange_Scale", atkRangeScale);
        AttackMove = PlayerPrefs.GetFloat("Attack_Move", AttackMove);
        atkRangeScale = PlayerPrefs.GetFloat("Attack_Speed", atkRangeScale);
        SecondAtkRangeScale = PlayerPrefs.GetFloat("SecondAtkRangeScale", SecondAtkRangeScale);
        QSkillRangeScale = PlayerPrefs.GetFloat("QSkillRangeScale", QSkillRangeScale);
        ESkillRangeScale = PlayerPrefs.GetFloat("ESkillRangeScale", ESkillRangeScale);
        power = PlayerPrefs.GetInt("Power", power);
        attackAddness = PlayerPrefs.GetInt("Attack_Addness", attackAddness);
        invincibleDuration = PlayerPrefs.GetFloat("InvincibleDuration", invincibleDuration);
        DashCoolDown = PlayerPrefs.GetInt("DashCoolDown", DashCoolDown);
        QCoolDown = PlayerPrefs.GetInt("QCoolDown", QCoolDown);
        ECoolDown = PlayerPrefs.GetInt("ECoolDown", ECoolDown);
        SecondDamageScale = PlayerPrefs.GetFloat("SecondDamageScale", SecondDamageScale);
        QSkillDamageScale = PlayerPrefs.GetFloat("QSkillDamageScale", QSkillDamageScale);
        ESkillDamageScale = PlayerPrefs.GetFloat("ESkillDamageScale", ESkillDamageScale);
        SkillTree1 = PlayerPrefs.GetInt("SkillTree1", SkillTree1);
        SkillTree3 = PlayerPrefs.GetInt("SkillTree3", SkillTree3);
        SkillTree5 = PlayerPrefs.GetInt("SkillTree5", SkillTree5);
        SkillTree7 = PlayerPrefs.GetInt("SkillTree7", SkillTree7);
        SkillTree10 = PlayerPrefs.GetInt("SkillTree10", SkillTree10);
        SkillTree13 = PlayerPrefs.GetInt("SkillTree13", SkillTree13);
        SkillTree15 = PlayerPrefs.GetInt("SkillTree15", SkillTree15);
        SkillTree18 = PlayerPrefs.GetInt("SkillTree18", SkillTree18);
        SkillTree20 = PlayerPrefs.GetInt("SkillTree20", SkillTree20);

    }
    private void SavePlayerStats()
    {
        // ���� ���� PlayerPrefs�� ����
        PlayerPrefs.SetInt("playerLevel", playerLevel);
        PlayerPrefs.SetInt("playerExp", playerExp);
        PlayerPrefs.SetInt("maxExp", maxExp);
        PlayerPrefs.SetInt("Player_MaxHealth", maxHealth);
        PlayerPrefs.SetInt("Current_Health", currentHealth);
        PlayerPrefs.SetFloat("Movement_Speed", movementSpeed);
        PlayerPrefs.SetFloat("Dash_Velocity", dashVelocity);
        PlayerPrefs.SetFloat("DashingTime", dashingTime);
        PlayerPrefs.SetInt("dashStack", dashStack);
        PlayerPrefs.SetFloat("AtkRange_Scale", atkRangeScale);
        PlayerPrefs.SetFloat("SecondAtkRangeScale", SecondAtkRangeScale);
        PlayerPrefs.SetFloat("QSkillRangeScale", QSkillRangeScale);
        PlayerPrefs.SetFloat("ESkillRangeScale", ESkillRangeScale);
        PlayerPrefs.SetFloat("Attack_Move", AttackMove);
        PlayerPrefs.SetFloat("Attack_Speed", atkRangeScale);
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetInt("Attack_Addness", attackAddness);
        PlayerPrefs.SetFloat("InvincibleDuration", invincibleDuration);
        PlayerPrefs.SetInt("DashCoolDown", DashCoolDown);
        PlayerPrefs.SetInt("QCoolDown", QCoolDown);
        PlayerPrefs.SetInt("ECoolDown", ECoolDown);
        PlayerPrefs.SetFloat("SecondDamageScale", SecondDamageScale);
        PlayerPrefs.SetFloat("QSkillDamageScale", QSkillDamageScale);
        PlayerPrefs.SetFloat("ESkillDamageScale", ESkillDamageScale);
        PlayerPrefs.SetInt("SkillTree1", SkillTree1);
        PlayerPrefs.SetInt("SkillTree3", SkillTree3);
        PlayerPrefs.SetInt("SkillTree5", SkillTree5);
        PlayerPrefs.SetInt("SkillTree7", SkillTree7);
        PlayerPrefs.SetInt("SkillTree10", SkillTree10);
        PlayerPrefs.SetInt("SkillTree13", SkillTree13);
        PlayerPrefs.SetInt("SkillTree15", SkillTree15);
        PlayerPrefs.SetInt("SkillTree18", SkillTree18);
        PlayerPrefs.SetInt("SkillTree20", SkillTree20);
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
        HealthChanged?.Invoke();

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
        playerLevel = 1;

        playerExp = 0;

        maxExp = 10;

        maxHealth = 100;
        
        movementSpeed = 5f;
        
        dashVelocity = 5f;

        dashingTime = 0.9f;

        dashStack = 1;

        atkRangeScale = 1f;

        SecondAtkRangeScale = 1.5f;

        QSkillRangeScale = 0.8f;

        ESkillRangeScale = 2f;

        AttackMove = 2f;

        atkSpeed = 1f;
        
        power = 10;

        attackAddness = 1;

        invincibleDuration = 1f;

        DashCoolDown = 10;

        QCoolDown = 10;

        ECoolDown = 15;

        SecondDamageScale = 2f;

        QSkillDamageScale = 2f;

        ESkillDamageScale = 3f;

        SkillTree1 = 0;
        SkillTree3 = 0;
        SkillTree5 = 0;
        SkillTree7 = 0;
        SkillTree10 = 0;
        SkillTree13 = 0;
        SkillTree15 = 0;
        SkillTree18 = 0;
        SkillTree20 = 0;
        
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
    public void GainExperience(int amount)
    {
        playerExp += amount;

        ExpChanged?.Invoke();
        while (playerExp >= maxExp)
        {
            playerLevel++;
            playerExp -= maxExp;
            CalculateNextLevelXP();
        }
    }
    private void CalculateNextLevelXP()
    {
        maxExp = (int)(maxExp * 1.1f);
        ExpChanged?.Invoke();

    }
    public void MaxHpPlusMinus()
    {
        HealthChanged?.Invoke();
    }
}
