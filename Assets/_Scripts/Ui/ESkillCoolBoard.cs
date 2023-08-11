using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ESkillCoolBoard : MonoBehaviour
{
    public Image skillCoolImage; // E 스킬 쿨다운 이미지 참조
    public WarriorMovement warriorMovement; // 대쉬 쿨다운 정보를 가져오기 위해 워리어 움직임을 참조합니다.
    public WarriorStatus stats;

    private bool skillEAvailable = true; // E 스킬이 사용 가능한 상태인지 나타내는 플래그

    void Start()
    {
        skillCoolImage.fillAmount = 1; // 처음에 쿨다운 없음으로 셋팅
    }

    void Update()
    {
        if (warriorMovement.canUseSkillE != skillEAvailable)
        {
            skillEAvailable = warriorMovement.canUseSkillE; // E 스킬 사용 가능 상태를 업데이트
            if (!skillEAvailable) StartCoroutine(SkillCoolDownVisualize(stats.ECoolDown)); // 쿨다운 시작
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
