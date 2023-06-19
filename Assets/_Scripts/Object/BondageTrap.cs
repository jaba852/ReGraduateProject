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

            // "Player" 태그가 있는 오브젝트에만 효과 적용
            if (gameObject.CompareTag("Player"))
            {
                warriorStatus = GetComponent<WarriorStatus>();
                if (warriorStatus != null)
                {
                    originalSpeed = warriorStatus.movementSpeed;
                    warriorStatus.movementSpeed = 0f; // 이동속도를 0으로 설정하여 속박

                    Invoke("RemoveEffect", 2f); // 10초 후에 효과 제거
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
