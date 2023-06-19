using UnityEngine;

public class DoorStatus : MonoBehaviour
{
    public int maxHealth = 500; // ���� �ִ� ü��
    public int currentHealth; // ���� ���� ü��
    public float disableTime = 5f; // ��Ȱ��ȭ�� ���°� ���ӵǴ� �ð�

    private bool isDisabled = false; // ���� ��Ȱ��ȭ ���� ����
    private float disableTimer = 0f; // ��Ȱ��ȭ Ÿ�̸�

    private void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
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
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isDisabled)
        {
            currentHealth -= damageAmount; // ü�¿��� ���� ��������ŭ ����

            if (currentHealth <= 0)
            {
                // Door�� �ı��Ǿ��� ���� ó���� ���⿡ �߰�

                // Door�� ��Ȱ��ȭ ���·� ��ȯ
                isDisabled = true;
                disableTimer = 0f;
                SetChildObjectsActive(false); // �ڽ� ������Ʈ ��Ȱ��ȭ
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
