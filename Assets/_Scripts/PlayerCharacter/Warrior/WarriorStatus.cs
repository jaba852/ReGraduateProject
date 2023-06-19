using System.Collections;
using UnityEngine;

public class WarriorStatus : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerRenderer;

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

    [Header("공격 범위 배율")]
    [SerializeField] public float atkRangeRatio = 1f;

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
    [SerializeField] public float DashCoolDown = 3f;

    public bool deadCount;

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
        // 스텟 값을 PlayerPrefs에 저장
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

}
