using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Button skillButton; // Add this line
    public Image[] otherSkillsToDisable;
    public int skillCost = 1;

    protected WarriorStatus warriorStatus;
    protected PointSystem pointSystem;

    protected virtual void Start()
    {
        warriorStatus = FindObjectOfType<WarriorStatus>();
        pointSystem = PointSystem.Instance;

        skillButton.onClick.AddListener(UseSkill); // Add this line
    }

    public void UseSkill()
    {
        if (pointSystem.CurrentPoints >= skillCost)
        {
            ApplyEffect();
            pointSystem.CurrentPoints -= skillCost;

            foreach (Image skill in otherSkillsToDisable)
            {
                skill.enabled = false;
            }
        }
    }

    protected virtual void ApplyEffect()
    {
        // 자식 클래스에서 이 메서드를 오버라이드합니다.
    }
}
