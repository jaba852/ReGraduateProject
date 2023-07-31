using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Button skillButton; // 버튼 컴포넌트 참조를 위한 변수 선언
    public Image[] otherSkillsToDisable; // 비활성화할 다른 스킬들에 대한 참조
    public float skillCost = 1; // 스킬 사용 비용

    protected WarriorStatus warriorStatus;
    protected PointSystem pointSystem;

    protected virtual void Start()
    {
        warriorStatus = FindObjectOfType<WarriorStatus>(); // 전사 상태에 대한 참조 획득
        //pointSystem = PointSystem.Instance; // 포인트 시스템 인스턴스 획득

        skillButton.onClick.AddListener(UseSkill); // 스킬 사용을 버튼 클릭 이벤트 리스너에 추가
    }

    public void UseSkill()
    {
        if (pointSystem.CurrentPoints >= skillCost) // 현재 포인트가 스킬 비용보다 많거나 같을 경우
        {
            ApplyEffect(); // 스킬 효과 적용
            pointSystem.CurrentPoints -= Mathf.FloorToInt(skillCost); // 스킬 비용만큼 포인트 감소

            //foreach (Image skill in otherSkillsToDisable) // 다른 스킬들을 비활성화
           // {
           //     skill.enabled = false;
           // }

            skillCost *= 1.2f; // 스킬 비용 20% 증가
        }
    }

    protected virtual void ApplyEffect()
    {
        // 이 메서드는 자식 클래스에서 오버라이드됩니다.
    }
}
