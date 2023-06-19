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
        // AttackSpeedBoost 스킬이 사용될 때 전사의 공격 속도를 증가시킵니다.
        warriorStatus.atkSpeed += AttackSpeedBoostAmount;

        Skill1 skill1 = FindObjectOfType<Skill1>() as Skill1;
        if (skill1 != null)
        {
            Button skill1Button = skill1.GetComponent<Button>();
            if (skill1Button != null)
            {
                skill1Button.interactable = false; // 버튼 비활성화
            }
        }

        Skill2 skill2 = FindObjectOfType<Skill2>() as Skill2;
        if (skill2 != null)
        {
            Button skill2Button = skill2.GetComponent<Button>();
            if (skill2Button != null)
            {
                skill2Button.interactable = false; // 버튼 비활성화
            }
        }

        skillButton.interactable = true; // 버튼 활성화
    }
}
