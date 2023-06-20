using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowManager : MonoBehaviour
{
    public Text SkilPoint; // �ؽ�Ʈ UI ��Ҹ� �����ϱ� ���� ����
    public Canvas uiCanvas;
    private PointSystem pointSystem; // ����Ʈ �ý����� �����ϱ� ���� ����

    void Awake()
    {
        uiCanvas = GetComponent<Canvas>();
        pointSystem = PointSystem.Instance; // PointSystem �ν��Ͻ��� �����ɴϴ�
        uiCanvas.enabled = false; // uiCanvas�� ��Ȱ��ȭ ���·� �����մϴ�
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            uiCanvas.enabled = !uiCanvas.enabled;
            SkilPoint.text = pointSystem.CurrentPoints.ToString(); // ����Ʈ �ý����� CurrentPoints �Ӽ��� �̿��� ���� ����Ʈ�� �����ɴϴ�
        }
    }
}
