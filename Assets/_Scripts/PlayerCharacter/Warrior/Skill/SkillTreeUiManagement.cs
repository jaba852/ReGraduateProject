using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUiManagement : MonoBehaviour
{
    public GameObject[] canvases;
    public Button[] buttons;

    public WarriorStatus level;

    private void Start()
    {
        // 초기에는 1번 버튼에 해당하는 캔버스를 활성화합니다.
        ActivateCanvas(0);

        // 버튼에 클릭 이벤트를 연결합니다.
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 클로저를 사용하여 올바른 인덱스를 전달합니다.
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        UpdateButtonStates();
    }

    // 선택된 캔버스를 활성화하고 나머지 캔버스들은 비활성화하는 함수입니다.
    private void ActivateCanvas(int index)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (i == index)
                canvases[i].SetActive(true);
            else
                canvases[i].SetActive(false);
        }
    }

    // 버튼 클릭 시 호출되는 함수
    private void OnButtonClick(int buttonIndex)
    {
        UpdateButtonStates();
        ActivateCanvas(buttonIndex);
    }

    // 버튼 활성화 상태 업데이트 함수
    private void UpdateButtonStates()
    {
        buttons[0].interactable = level.playerLevel >= 1;
        buttons[1].interactable = level.playerLevel >= 3;
        buttons[2].interactable = level.playerLevel >= 5;
        buttons[3].interactable = level.playerLevel >= 7;
        buttons[4].interactable = level.playerLevel >= 10;
        buttons[5].interactable = level.playerLevel >= 13;
        buttons[6].interactable = level.playerLevel >= 15;
        buttons[7].interactable = level.playerLevel >= 18;
        buttons[8].interactable = level.playerLevel >= 20;
    }
}
