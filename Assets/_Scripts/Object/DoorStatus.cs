using UnityEngine;

public class DoorStatus : MonoBehaviour
{
    public int maxHealth = 30; // ���� �ִ� ü��
    public int currentHealth; // ���� ���� ü��
    public float disableTime = 5f; // ��Ȱ��ȭ�� ���°� ���ӵǴ� �ð�
    //�߰��� �κ�
    public Sprite defaultSprite; // �⺻ ��������Ʈ
    public Sprite disabledSprite; // ��Ȱ��ȭ�� ��������Ʈ

    private bool isDisabled = false; // ���� ��Ȱ��ȭ ���� ����
    private float disableTimer = 0f; // ��Ȱ��ȭ Ÿ�̸�
    //�߰��� �κ�
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������ ������Ʈ

    public float shakeAmount = 0.1f; // ��鸲 ����
    public float shakeSpeed = 0.1f; // ��鸲 �ӵ�
    public float shakeDuration = 0.1f; // ��鸲 ���� �ð�
    public float moveDistance = 0.1f; // �̵� �Ÿ�
    public Vector2 initialPosition;
    private void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
        //�߰��� �κ�
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ������Ʈ ��������
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isDisabled)
        {
            disableTimer += Time.deltaTime; // ��Ȱ��ȭ Ÿ�̸� ����

            if (disableTimer >= disableTime)
            {
                // ���� �ð��� ������ �ٽ� Ȱ��ȭ
                isDisabled = false;
                disableTimer = 0f;
                currentHealth = maxHealth; // ü���� �ִ� ü������ ����
                SetChildObjectsActive(true); // �ڽ� ������Ʈ Ȱ��ȭ
                //�߰��� �κ�
                spriteRenderer.sprite = defaultSprite; // �⺻ ��������Ʈ�� ����
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isDisabled)
        {
            currentHealth -= damageAmount; // ü�¿��� ���� ��������ŭ ����
            StartCoroutine(ShakeObject());

            if (currentHealth <= 0)
            {
                // Door�� �ı��Ǿ��� ���� ó���� ���⿡ �߰�

                // Door�� ��Ȱ��ȭ ���·� ��ȯ
                isDisabled = true;
                disableTimer = 0f;
                SetChildObjectsActive(false); // �ڽ� ������Ʈ ��Ȱ��ȭ
                //�߰��� �κ�
                spriteRenderer.sprite = disabledSprite; // ��Ȱ��ȭ�� ��������Ʈ�� ����
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

            // ������ �������� ���� ���� �������� �̵�
            if (elapsedTime % 0.2f < 0.1f)
            {
                transform.position += new Vector3(moveDirection * moveDistance, 0f, 0f);
                moveDirection *= -1f; // �̵� ���� ����
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��鸲�� ���� ��, �ʱ� ��ġ�� �ǵ�����
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
