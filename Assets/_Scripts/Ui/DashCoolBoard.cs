using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashCoolBoard : MonoBehaviour
{
    public Image dashCoolImage;  // 대쉬 쿨다운 이미지 참조
    public WarriorMovement warriorMovement;  // 대쉬 쿨다운 정보를 가져오기 위해 워리어 움직임을 참조합니다.
    public WarriorStatus stats;

    private bool dashAvailable = true;  // 대쉬가 사용 가능한 상태인지 나타내는 플래그

    void Start()
    {
        dashCoolImage.fillAmount = 1;  // 처음에 쿨다운 없음으로 셋팅
    }

    void Update()
    {

        if (warriorMovement.isCooldownRunning != dashAvailable)
        {
            dashAvailable = warriorMovement.isCooldownRunning;  // 대쉬 쿨다운 상태를 업데이트
            if (dashAvailable) StartCoroutine(DashCoolDownVisualize());  // 쿨다운 시작
        }

    }

    // 쿨다운이 동작하는 동안 쿨타임 바를 조정하는 코루틴
    private IEnumerator DashCoolDownVisualize()
    {
        float coolDownTime = stats.DashCoolDown;
        float passedTime = 0;

        while (passedTime < coolDownTime)
        {
            passedTime += Time.deltaTime;
            dashCoolImage.fillAmount = 1 - (passedTime / coolDownTime);  // 남은 쿨타임 비율로 쿨타임 바를 조정
            yield return null;
        }

        dashCoolImage.fillAmount = 1;  // 쿨다운이 끝나면 다시 쿨다운 이미지를 꽉 채움
    }
}