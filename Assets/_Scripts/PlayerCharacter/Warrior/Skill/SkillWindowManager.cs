using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowManager : MonoBehaviour
{
    public Text SkilPoint; // 텍스트 UI 요소를 참조하기 위한 변수
    public Canvas uiCanvas;
    private PointSystem pointSystem; // 포인트 시스템을 참조하기 위한 변수

    void Awake()
    {
        uiCanvas = GetComponent<Canvas>();
        pointSystem = PointSystem.Instance; // PointSystem 인스턴스를 가져옵니다
        uiCanvas.enabled = false; // uiCanvas를 비활성화 상태로 설정합니다
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            uiCanvas.enabled = !uiCanvas.enabled;
            SkilPoint.text = pointSystem.CurrentPoints.ToString(); // 포인트 시스템의 CurrentPoints 속성을 이용해 현재 포인트를 가져옵니다
        }
    }
}
