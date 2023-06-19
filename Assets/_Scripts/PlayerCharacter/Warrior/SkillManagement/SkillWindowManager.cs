using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowManager : MonoBehaviour
{
    public Text SkilPoint; // �ؽ�Ʈ UI ��Ҹ� �����ϱ� ���� ����
    private Canvas uiCanvas;
    private PointSystem pointSystem; // ����Ʈ �ý����� �����ϱ� ���� ����

    void Awake()
    {
        uiCanvas = GetComponent<Canvas>();
        pointSystem = PointSystem.Instance; // PointSystem �ν��Ͻ��� �����ɴϴ�
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            uiCanvas.enabled = !uiCanvas.enabled;
            SkilPoint.text = "���� ��ų����Ʈ: " + pointSystem.CurrentPoints.ToString(); // ����Ʈ �ý����� CurrentPoints �Ӽ��� �̿��� ���� ����Ʈ�� �����ɴϴ�
        }
    }
}
