using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCanvas : MonoBehaviour
{
    public Canvas uiCanvas; // ���� ĵ����
    public WarriorStatus warriorStatus; // ������ ����

    private void Update()
    {
        if (warriorStatus.deadCount) // ����� �׾����� Ȯ��
        {
            uiCanvas.gameObject.SetActive(false); // ĵ������ ����
        }
        else
        {
            uiCanvas.gameObject.SetActive(true); // ����� ��� ������ ĵ������ ǥ��
        }
    }
}
