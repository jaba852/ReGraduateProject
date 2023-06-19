using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramove : MonoBehaviour
{
    private Transform target; // ĳ���� Transform ������Ʈ
    public float smoothSpeed = 0.125f; // ī�޶� �̵� �ӵ�
    private Vector3 cameraOffset = new Vector3(0f, 0f, -10f); // ī�޶� ��ġ Offset

    private WarriorStatus warriorStatus; // WarriorStatus ��ũ��Ʈ ����

    private bool isShaking = false; // ���� ȿ�� ������ ����
    private float shakeDuration = 0f; // ���� ���� �ð�
    private float shakeMagnitude = 0.1f; // ���� ����
    private float shakeTimer = 0f; // ���� Ÿ�̸�
    private float previousHealth; // ���� �����ӿ����� ü��

    void Start()
    {
        FindPlayer(); // player�� ã�� �Լ� ȣ��
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

    // ���� ȿ�� ����
    private void StartCameraShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            shakeTimer = 0f;
            shakeDuration = 0.2f; // ���� ���� �ð� ���� (���ϴ� ������ ����)
            shakeMagnitude = 0.08f; // ���� ���� ���� (���ϴ� ������ ����)
        }
    }
    // Player�� ã�� �Լ�
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

    // Player�� ã�� ���� ��� ���������� ã�� ���� �ڷ�ƾ
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
