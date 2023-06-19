using System.Collections;
using UnityEngine;


public class DarknessTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DarknessTrapEffect effect = collision.GetComponent<DarknessTrapEffect>();
            if (effect == null)
            {
                effect = collision.gameObject.AddComponent<DarknessTrapEffect>();
                effect.ApplyEffect();
            }
        }
    }
}

public class DarknessTrapEffect : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D warriorLight;
    private float originalInnerRadius;
    private float originalOuterRadius;
    private bool isActive = false;

    private void Start()
    {
        warriorLight = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
    }

    public void ApplyEffect()
    {
        if (!isActive)
        {
            isActive = true;

            // "Player" �±װ� �ִ� ������Ʈ���� ȿ�� ����
            if (gameObject.CompareTag("Player"))
            {
                warriorLight = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
                if (warriorLight != null)
                {
                    originalInnerRadius = warriorLight.pointLightInnerRadius;
                    originalOuterRadius = warriorLight.pointLightOuterRadius;

                    warriorLight.pointLightInnerRadius = 1.5f; // InnerRadius ���� 1�� ����
                    warriorLight.pointLightOuterRadius = 1.5f; // OuterRadius ���� 1�� ����

                    Invoke("RemoveEffect", 10f); // 10�� �Ŀ� ȿ�� ����
                }
            }
        }
    }

    private void RemoveEffect()
    {
        isActive = false;
        warriorLight.pointLightInnerRadius = originalInnerRadius;
        warriorLight.pointLightOuterRadius = originalOuterRadius;

        Destroy(this);
    }
}
