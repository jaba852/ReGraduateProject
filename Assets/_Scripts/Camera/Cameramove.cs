using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramove : MonoBehaviour
{
    private Transform target; // 캐릭터 Transform 컴포넌트
    public float smoothSpeed = 0.125f; // 카메라 이동 속도
    private Vector3 cameraOffset = new Vector3(0f, 0f, -10f); // 카메라 위치 Offset

    private WarriorStatus warriorStatus; // WarriorStatus 스크립트 참조

    private bool isShaking = false; // 진동 효과 중인지 여부
    private float shakeDuration = 0f; // 진동 지속 시간
    private float shakeMagnitude = 0.1f; // 진동 세기
    private float shakeTimer = 0f; // 진동 타이머
    private float previousHealth; // 이전 프레임에서의 체력

    void Start()
    {
        FindPlayer(); // player를 찾는 함수 호출
        if (target != null)
        {
            warriorStatus = target.GetComponent<WarriorStatus>();
        }
        if (warriorStatus != null)
        {
            previousHealth = (float)warriorStatus.currentHealth;
        }
    }

    void LateUpdate()
    {
        if (warriorStatus != null)
        {
            if (warriorStatus.currentHealth < previousHealth)
            {
                StartCameraShake();
            }
            previousHealth = (float)warriorStatus.currentHealth;
            Debug.Log(previousHealth);
        }

        Vector3 desiredPosition = target.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        if (isShaking)
        {
            if (shakeTimer < shakeDuration)
            {
                Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
                smoothedPosition += new Vector3(shakeOffset.x, shakeOffset.y, 0f);
                shakeTimer += Time.deltaTime;
            }
            else
            {
                isShaking = false;
            }
        }

        transform.position = smoothedPosition;
    }

    // 진동 효과 시작
    private void StartCameraShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            shakeTimer = 0f;
            shakeDuration = 0.2f; // 진동 지속 시간 설정 (원하는 값으로 조정)
            shakeMagnitude = 0.08f; // 진동 세기 설정 (원하는 값으로 조정)
        }
    }
    // Player를 찾는 함수
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            StartCoroutine(SearchForPlayer());
        }
    }

    // Player를 찾지 못한 경우 지속적으로 찾기 위한 코루틴
    private IEnumerator SearchForPlayer()
    {
        while (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                warriorStatus = player.GetComponent<WarriorStatus>();
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (warriorStatus != null)
        {
            previousHealth = (float)warriorStatus.currentHealth;
        }
    }
}
