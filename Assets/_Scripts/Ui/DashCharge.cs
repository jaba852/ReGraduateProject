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
        // WarriorMovement ��ũ��Ʈ ��������
        warriorMovement = GetComponentInParent<WarriorMovement>();

        // Text ������Ʈ ��������
        dashChargeText = GetComponent<Text>();

        // �뽬 ���� ��ġ�� UI�� ǥ��
        UpdateDashChargeText();
    }

    private void Update()
    {
        // �뽬 ���� ��ġ�� ����Ǹ� UI ������Ʈ
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
