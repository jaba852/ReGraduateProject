using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill3 : Skill
{
    public int AttackSpeedBoostAmount = 10;

    protected override void Start()
    {
        base.Start();
        skillButton = GetComponent<Button>();
    }


    protected override void ApplyEffect()
    {
        // AttackSpeedBoost ��ų�� ���� �� ������ ���� �ӵ��� ������ŵ�ϴ�.
        warriorStatus.atkSpeed += AttackSpeedBoostAmount;

        Skill1 skill1 = FindObjectOfType<Skill1>() as Skill1;
        if (skill1 != null)
        {
            Button skill1Button = skill1.GetComponent<Button>();
            if (skill1Button != null)
            {
                skill1Button.interactable = false; // ��ư ��Ȱ��ȭ
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
