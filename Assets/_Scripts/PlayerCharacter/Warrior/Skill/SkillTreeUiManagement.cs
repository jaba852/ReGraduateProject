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
        // �ʱ⿡�� 1�� ��ư�� �ش��ϴ� ĵ������ Ȱ��ȭ�մϴ�.
        ActivateCanvas(0);

        // ��ư�� Ŭ�� �̺�Ʈ�� �����մϴ�.
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Ŭ������ ����Ͽ� �ùٸ� �ε����� �����մϴ�.
            buttons[i].onClick.AddListener(() => ActivateCanvas(index));
        }
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
}
