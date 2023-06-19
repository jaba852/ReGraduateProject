using System.Collections;
using UnityEngine;

public class BondageTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BondageTrapEffect effect = collision.GetComponent<BondageTrapEffect>();
            if (effect == null)
            {
                effect = collision.gameObject.AddComponent<BondageTrapEffect>();
                effect.ApplyEffect();
            }
        }
    }
}

public class BondageTrapEffect : MonoBehaviour
{
    private WarriorStatus warriorStatus;
    private float originalSpeed;
    private bool isActive = false;

    private void Start()
    {
        warriorStatus = GetComponent<WarriorStatus>();
    }

    public void ApplyEffect()
    {
        if (!isActive)
        {
            isActive = true;

            // "Player" �±װ� �ִ� ������Ʈ���� ȿ�� ����
            if (gameObject.CompareTag("Player"))
            {
                warriorStatus = GetComponent<WarriorStatus>();
                if (warriorStatus != null)
                {
                    originalSpeed = warriorStatus.movementSpeed;
                    warriorStatus.movementSpeed = 0f; // �̵��ӵ��� 0���� �����Ͽ� �ӹ�

                    Invoke("RemoveEffect", 2f); // 10�� �Ŀ� ȿ�� ����
                }
            }
        }
    }

    private void RemoveEffect()
    {
        isActive = false;
        warriorStatus.movementSpeed = originalSpeed;

        Destroy(this);
    }
}
