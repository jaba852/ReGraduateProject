using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUiManagement : MonoBehaviour
{
    public GameObject[] canvases;
    public Button[] buttons;

    private void Start()
    {
        // 초기에는 1번 버튼에 해당하는 캔버스를 활성화합니다.
        ActivateCanvas(0);

        // 버튼에 클릭 이벤트를 연결합니다.
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 클로저를 사용하여 올바른 인덱스를 전달합니다.
            buttons[i].onClick.AddListener(() => ActivateCanvas(index));
        }
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
}
