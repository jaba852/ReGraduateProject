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
        // HealthBoost 스킬이 사용될 때 전사의 최대 체력을 증가시킵니다.
        warriorStatus.maxHealth += healthBoostAmount;

        //Skill3 skill3 = FindObjectOfType<Skill3>() as Skill3;
        //if (skill3 != null)
        //{
        //    Button skill3Button = skill3.GetComponent<Button>();
        //    if (skill3Button != null)
        //    {
        //        skill3Button.interactable = false; // 버튼 비활성화
        //    }
        //}

        //Skill2 skill2 = FindObjectOfType<Skill2>() as Skill2;
        //if (skill2 != null)
        //{
        //    Button skill2Button = skill2.GetComponent<Button>();
        //    if (skill2Button != null)
        //    {
        //        skill2Button.interactable = false; // 버튼 비활성화
        //    }
        //}

        skillButton.interactable = false; // 버튼을 비활성화하지 않고 사용자 입력을 막습니다.
        skillButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.5f);  // 버튼의 이미지 색상을 검은색으로 변경합니다.
    }
}