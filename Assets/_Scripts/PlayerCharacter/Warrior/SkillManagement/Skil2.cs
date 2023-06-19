using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill2 : Skill
{
    
    public int AttackBoostAmount = 10;

    protected override void Start()
    {
        base.Start();
        skillButton = GetComponent<Button>();
    }

    protected override void ApplyEffect()
    {
        // AttackBoost 스킬이 사용될 때 전사의 공격력을 증가시킵니다.
        warriorStatus.power += AttackBoostAmount;

        Skill3 skill3 = FindObjectOfType<Skill3>() as Skill3;
        if (skill3 != null)
        {
            Button skill3Button = skill3.GetComponent<Button>();
            if (skill3Button != null)
            {
                skill3Button.interactable = false; // 버튼 비활성화
            }
        }

        Skill1 skill1 = FindObjectOfType<Skill1>() as Skill1;
        if (skill1 != null)
        {
            Button skill1Button = skill1.GetComponent<Button>();
            if (skill1Button != null)
            {
                skill1Button.interactable = false; // 버튼 비활성화
            }
        }

        skillButton.interactable = true; // 버튼 활성화
    }
}
