using System;
using System.Collections;
using UnityEngine;

public class WarriorStatus : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerRenderer;

    [Header("플레이어 레벨")]
    [SerializeField] public int playerLevel = 1;

    [Header("플레이어 보유 경험치")]
    [SerializeField] public int playerExp = 0;

    [Header("플레이어 요구 경험치")]
    [SerializeField] public int maxExp = 10;

    [Header("피격 깜빡이 시간")]
    [SerializeField] public float blinkTime = 1f;

    [Header("최대체력")]
    [SerializeField] public int maxHealth = 100;

    [Header("현재체력")]
    [SerializeField] public int currentHealth;

    [Header("이동속도")]
    [SerializeField] public float movementSpeed = 5f;

    [Header("대쉬 가속도")]
    [SerializeField] public float dashVelocity = 5f;

    [Header("대쉬 지속시간")]
    [SerializeField] public float dashingTime = 0.9f;

    [Header("대쉬 스택")]
    [SerializeField] public int dashStack = 1;

    [Header("기본공격 범위 배율")]
    [SerializeField] public float atkRangeScale = 1f;

    [Header("두번째 기본공격 범위 배율")]
    [SerializeField] public float SecondAtkRangeScale = 1.5f;

    [Header("Q스킬 공격 범위 배율")]
    [SerializeField] public float QSkillRangeScale = 0.8f;

    [Header("E스킬 공격 범위 배율")]
    [SerializeField] public float ESkillRangeScale = 2f;

    [Header("공격 이동거리")]
    [SerializeField] public float AttackMove = 2f;

    [Header("공격 속도")]
    [SerializeField] public float atkSpeed = 1f;

    [Header("공격력")]
    [SerializeField] public int power = 10;

    [Header("추가데미지")]
    [SerializeField] public int attackAddness = 1;

    [Header("피격무적시간")]
    [SerializeField] public float invincibleDuration = 1f;

    [Header("구르기 쿨타임")]
    [SerializeField] public int DashCoolDown = 10;

    [Header("Q스킬 쿨타임")]
    [SerializeField] public int QCoolDown = 10;

    [Header("E스킬 쿨타임")]
    [SerializeField] public int ECoolDown = 15;

    [Header("2타 기본공격 배율")]
    [SerializeField] public float SecondDamageScale = 2f;

    [Header("Q스킬 데미지 배수")]
    [SerializeField] public float QSkillDamageScale = 2f;

    [Header("E스킬 데미지 배수")]
    [SerializeField] public float ESkillDamageScale = 3f;


    [Header("여기서부터 스킬트리 0이라면 미습득 1,2,3이라면 해당하는걸 습득")]
    [SerializeField] public int SkillTree1 = 0;
    [SerializeField] public int SkillTree3 = 0;
    [SerializeField] public int SkillTree5 = 0;
    [SerializeField] public int SkillTree7 = 0;
    [SerializeField] public int SkillTree10 = 0;
    [SerializeField] public int SkillTree13 = 0;
    [SerializeField] public int SkillTree15 = 0;
    [SerializeField] public int SkillTree18 = 0;
    [SerializeField] public int SkillTree20 = 0;

    [Header("죽었는가")]
    public bool deadCount;


    public event Action HealthChanged;

    public event Action ExpChanged;

    private bool isInvincible = false; // 피격 무적 상태를 나타내는 변수

    private void Start()
    {
        // 스텟 초기화
        LoadPlayerStats();

        deadCount = false;
    }
    private void OnDestroy()
    {
        // 게임 종료 또는 씬 전환 시 스텟 저장
        SavePlayerStats();
    }
    private void LoadPlayerStats()
    {
        currentHealth = maxHealth;
        // 스텟 값들을 PlayerPrefs에서 불러옴
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
        // 스텟 값을 PlayerPrefs에 저장
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
        // PlayerPrefs 변경 사항을 저장
        PlayerPrefs.Save();
    }

    public void TakeDamagePlayer(int damage)
    {
        if (isInvincible || deadCount || WarriorMovement.isDashing)
        {
            return; // 피격무적, 사망, 대쉬 중에 무적
        }

        currentHealth -= damage;
        Debug.Log("플레이어 피격" + damage);
        HealthChanged?.Invoke();

        if (currentHealth <= 0)
        {
            deadCount = true;
            GetComponent<Animator>().SetBool("isDead", deadCount);
            InitializePlayerStats(); // 스텟 초기화
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
        playerRenderer.enabled = true; // 피격 당한 후 플레이어를 다시 활성화합니다.


    }
    private IEnumerator invincibility()
    {
        isInvincible = true; // 피격 무적 상태 활성화

        // 피격 효과, 애니메이션 등 추가 처리

        yield return new WaitForSeconds(invincibleDuration); // 피격 무적 지속 시간만큼 대기

        isInvincible = false; // 피격 무적 상태 비활성화
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
        
        // 플레이어의 체력을 초기화된 값으로 설정
        currentHealth = maxHealth;

        // CircleCollider2D 비활성화
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("CircleCollider2D 컴포넌트를 찾을 수 없습니다!");
        }

        // Rigidbody2D 비활성화
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.simulated = false;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D 컴포넌트를 찾을 수 없습니다!");
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
