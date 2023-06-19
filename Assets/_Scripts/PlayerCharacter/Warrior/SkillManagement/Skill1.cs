using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill1 : Skill
{
    public int healthBoostAmount = 10;

    protected override void Start()
    {
        base.Start();
        skillButton = GetComponent<Button>();
    }

    protected override void ApplyEffect()
    {
        // HealthBoost ��ų�� ���� �� ������ �ִ� ü���� ������ŵ�ϴ�.
        warriorStatus.maxHealth += healthBoostAmount;

        Skill3 skill3 = FindObjectOfType<Skill3>() as Skill3;
        if (skill3 != null)
        {
            Button skill3Button = skill3.GetComponent<Button>();
            if (skill3Button != null)
            {
                skill3Button.interactable = false; // ��ư ��Ȱ��ȭ
            }
        }

        Skill2 skill2 = FindObjectOfType<Skill2>() as Skill2;
        if (skill2 != null)
        {
            Button skill2Button = skill2.GetComponent<Button>();
            if (skill2Button != null)
            {
                skill2Button.interactable = false; // ��ư ��Ȱ��ȭ
            }
        }

        skillButton.interactable = true; // ��ư Ȱ��ȭ
    }
}