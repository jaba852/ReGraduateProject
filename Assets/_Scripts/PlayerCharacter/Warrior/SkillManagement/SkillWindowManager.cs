using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowManager : MonoBehaviour
{
    public Text SkilPoint; // 텍스트 UI 요소를 참조하기 위한 변수
    private Canvas uiCanvas;
    private PointSystem pointSystem; // 포인트 시스템을 참조하기 위한 변수

    void Awake()
    {
        uiCanvas = GetComponent<Canvas>();
        pointSystem = PointSystem.Instance; // PointSystem 인스턴스를 가져옵니다
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            uiCanvas.enabled = !uiCanvas.enabled;
            SkilPoint.text = "현재 스킬포인트: " + pointSystem.CurrentPoints.ToString(); // 포인트 시스템의 CurrentPoints 속성을 이용해 현재 포인트를 가져옵니다
        }
    }
}
