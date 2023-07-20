using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCharge : MonoBehaviour
{
    public WarriorMovement warriorMovement;
    public Text dashChargeText;

    private void Start()
    {
        // WarriorMovement 스크립트 가져오기
        warriorMovement = GetComponentInParent<WarriorMovement>();

        // Text 컴포넌트 가져오기
        dashChargeText = GetComponent<Text>();

        // 대쉬 충전 수치를 UI에 표시
        UpdateDashChargeText();
    }

    private void Update()
    {
        // 대쉬 충전 수치가 변경되면 UI 업데이트
        UpdateDashChargeText();
    }

    private void UpdateDashChargeText()
    {
        if (warriorMovement != null && dashChargeText != null)
        {
            dashChargeText.text = warriorMovement.dashCharges.ToString();
        }
    }
}
