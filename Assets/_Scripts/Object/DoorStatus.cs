using UnityEngine;

public class DoorStatus : MonoBehaviour
{
    public int maxHealth = 30; // 벽의 최대 체력
    public int currentHealth; // 벽의 현재 체력
    public float disableTime = 5f; // 비활성화된 상태가 지속되는 시간
    //추가된 부분
    public Sprite defaultSprite; // 기본 스프라이트
    public Sprite disabledSprite; // 비활성화된 스프라이트

    private bool isDisabled = false; // 벽의 비활성화 상태 여부
    private float disableTimer = 0f; // 비활성화 타이머
    //추가된 부분
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 컴포넌트

    public float shakeAmount = 0.1f; // 흔들림 정도
    public float shakeSpeed = 0.1f; // 흔들림 속도
    public float shakeDuration = 0.1f; // 흔들림 지속 시간
    public float moveDistance = 0.1f; // 이동 거리
    public Vector2 initialPosition;
    private void Start()
    {
        currentHealth = maxHealth; // 벽의 체력을 최대 체력으로 초기화
        //추가된 부분
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        initialPosition = transform.position;
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
            StartCoroutine(ShakeObject());

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

    private System.Collections.IEnumerator ShakeObject()
    {
        float elapsedTime = 0f;
        float moveDirection = -1f;

        while (elapsedTime < shakeDuration)
        {
            float offset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            Vector2 shakePosition = initialPosition + new Vector2(0f, offset);

            transform.position = new Vector2(initialPosition.x, shakePosition.y);

            // 투박한 움직임을 위해 일정 간격으로 이동
            if (elapsedTime % 0.2f < 0.1f)
            {
                transform.position += new Vector3(moveDirection * moveDistance, 0f, 0f);
                moveDirection *= -1f; // 이동 방향 반전
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 흔들림이 끝난 후, 초기 위치로 되돌리기
        transform.position = initialPosition;
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
