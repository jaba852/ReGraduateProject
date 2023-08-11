using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QSkillCoolBoard : MonoBehaviour
{
    public Image skillCoolImage; // Q 스킬 쿨다운 이미지 참조
    public WarriorMovement warriorMovement;
    public WarriorStatus stats;

    private bool skillQAvailable = true; // Q 스킬이 사용 가능한 상태인지 나타내는 플래그

    void Start()
    {
        skillCoolImage.fillAmount = 1; // 처음에 쿨다운 없음으로 셋팅
    }

    void Update()
    {
        if (warriorMovement.canUseSkillQ != skillQAvailable)
        {
            skillQAvailable = warriorMovement.canUseSkillQ; // Q 스킬 사용 가능 상태를 업데이트
            if (!skillQAvailable) StartCoroutine(SkillCoolDownVisualize(stats.QCoolDown)); // 쿨다운 시작
        }
    }

    // 쿨다운이 동작하는 동안 쿨타임 바를 조정하는 코루틴
    private IEnumerator SkillCoolDownVisualize(float coolDownTime)
    {
        float passedTime = 0;

        while (passedTime < coolDownTime)
        {
            passedTime += Time.deltaTime;
            skillCoolImage.fillAmount = 1 - (passedTime / coolDownTime); // 남은 쿨타임 비율로 쿨타임 바를 조정
            yield return null;
        }

        skillCoolImage.fillAmount = 1; // 쿨다운이 끝나면 다시 쿨다운 이미지를 꽉 채움
    }
}
