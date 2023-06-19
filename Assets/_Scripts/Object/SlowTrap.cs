using System.Collections;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SlowTrapEffect effect = collision.GetComponent<SlowTrapEffect>();
            if (effect == null)
            {
                effect = collision.gameObject.AddComponent<SlowTrapEffect>();
                effect.ApplyEffect();
            }
        }
    }
}

public class SlowTrapEffect : MonoBehaviour
{
    private WarriorStatus warriorStatus;
    private float originalSpeed;
    private float slowedSpeed;
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
                    slowedSpeed = originalSpeed * 0.5f; // �̵��ӵ��� ?%�� ����

                    warriorStatus.movementSpeed = slowedSpeed;

                    Invoke("RemoveEffect", 10f); // 10�� �Ŀ� ȿ�� ����
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
