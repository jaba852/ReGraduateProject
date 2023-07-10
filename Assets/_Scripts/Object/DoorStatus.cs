using UnityEngine;

public class DoorStatus : MonoBehaviour
{
    public int maxHealth = 500; // 벽의 최대 체력
    public int currentHealth; // 벽의 현재 체력
    public float disableTime = 5f; // 비활성화된 상태가 지속되는 시간
    //추가된 부분
    public Sprite defaultSprite; // 기본 스프라이트
    public Sprite disabledSprite; // 비활성화된 스프라이트

    private bool isDisabled = false; // 벽의 비활성화 상태 여부
    private float disableTimer = 0f; // 비활성화 타이머
    //추가된 부분
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 컴포넌트

    private void Start()
    {
        currentHealth = maxHealth; // 벽의 체력을 최대 체력으로 초기화
        //추가된 부분
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
    }

    private void Update()
    {
        if (isDisabled)
        {
            disableTimer += Time.deltaTime; // 비활성화 타이머 갱신

            if (disableTimer >= disableTime)
            {
                // 일정 시간이 지나면 다시 활성화
                isDisabled = false;
                disableTimer = 0f;
                currentHealth = maxHealth; // 체력을 최대 체력으로 복구
                SetChildObjectsActive(true); // 자식 오브젝트 활성화
                //추가된 부분
                spriteRenderer.sprite = defaultSprite; // 기본 스프라이트로 변경
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isDisabled)
        {
            currentHealth -= damageAmount; // 체력에서 받은 데미지만큼 감소

            if (currentHealth <= 0)
            {
                // Door가 파괴되었을 때의 처리를 여기에 추가

                // Door를 비활성화 상태로 전환
                isDisabled = true;
                disableTimer = 0f;
                SetChildObjectsActive(false); // 자식 오브젝트 비활성화
                //추가된 부분
                spriteRenderer.sprite = disabledSprite; // 비활성화된 스프라이트로 변경
            }
        }
    }

    private void SetChildObjectsActive(bool isActive)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject childObject = transform.GetChild(i).gameObject;
            childObject.SetActive(isActive);
        }
    }
}
