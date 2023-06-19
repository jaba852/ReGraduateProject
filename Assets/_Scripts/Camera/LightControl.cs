using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightControl : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D characterLight;
    public WarriorStatus warriorStatus;

    void Start()
    {
        characterLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    void Update()
    {
        if (warriorStatus.currentHealth <= 0)
        {
            // ü���� 0�� �� ����Ʈ�� ���� ���������� ����
            characterLight.color = new Color(1, 0, 0);
        }
        else if (warriorStatus.currentHealth <= warriorStatus.maxHealth * 0.2f)
        {
            // ü���� 20% �̸��� �� ����Ʈ�� ���� �Ӱ� ����
            characterLight.color = new Color(1, 0.5f, 0.6f);
        }
        else
        {
            // ü���� 20% �̻��� ���� ���� ������
            characterLight.color = Color.white;
        }
    }
}
