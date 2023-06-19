using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill4 : Skill
{
    public int AttackSpeedBoostAmount = 10;
    internal object skillImage;

    protected override void ApplyEffect()
    {
        // Boost 스킬이 사용될 때 전사의  증가시킵니다.
        warriorStatus.atkSpeed += AttackSpeedBoostAmount;

        Skill2 skill2 = FindObjectOfType<Skill2>();
        if (skill2 != null)
        {
            skill2.gameObject.SetActive(false);
        }

        Skill3 skill3 = FindObjectOfType<Skill3>();
        if (skill3 != null)
        {
            skill3.gameObject.SetActive(false);
        }

        gameObject.SetActive(true); // Skill를 활성화합니다.
    }

    
}
