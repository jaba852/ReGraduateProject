using System.Collections;
using UnityEngine;

public class AtkTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AtkTrapEffect effect = collision.GetComponent<AtkTrapEffect>();
            if (effect == null)
            {
                effect = collision.gameObject.AddComponent<AtkTrapEffect>();
                effect.ApplyEffect();
            }
        }
    }
}

public class AtkTrapEffect : MonoBehaviour
{
    private WarriorStatus warriorStatus;
    private float originalAtkSpeed;
    private float slowedAtkSpeed;
    private int originalPower;
    private int slowedPower;
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
                    originalAtkSpeed = warriorStatus.atkSpeed;
                    slowedAtkSpeed = originalAtkSpeed * 0.5f; // ���� �ӵ��� 50%�� ����

                    originalPower = warriorStatus.power;
                    slowedPower = Mathf.RoundToInt(originalPower * 0.5f); // ���ݷ��� 50%�� ����

                    warriorStatus.atkSpeed = slowedAtkSpeed;
                    warriorStatus.power = slowedPower;

                    Invoke("RemoveEffect", 10f); // 10�� �Ŀ� ȿ�� ����
                }
            }
        }
    }

    private void RemoveEffect()
    {
        isActive = false;
        warriorStatus.atkSpeed = originalAtkSpeed;
        warriorStatus.power = originalPower;

        Destroy(this);
    }
}
