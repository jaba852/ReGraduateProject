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
        // �ʱ⿡�� 1�� ��ư�� �ش��ϴ� ĵ������ Ȱ��ȭ�մϴ�.
        ActivateCanvas(0);

        // ��ư�� Ŭ�� �̺�Ʈ�� �����մϴ�.
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Ŭ������ ����Ͽ� �ùٸ� �ε����� �����մϴ�.
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        UpdateButtonStates();
    }

    // ���õ� ĵ������ Ȱ��ȭ�ϰ� ������ ĵ�������� ��Ȱ��ȭ�ϴ� �Լ��Դϴ�.
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

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    private void OnButtonClick(int buttonIndex)
    {
        UpdateButtonStates();
        ActivateCanvas(buttonIndex);
    }

    // ��ư Ȱ��ȭ ���� ������Ʈ �Լ�
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
