using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill1_3 : MonoBehaviour
{
    public Button skillButton; // 1�� ��ų ��ư�� Reference�� Inspector���� �������ּ���.
    public Button skillButton2; // 2�� ��ų ��ư�� Reference�� Inspector���� �������ּ���.
    public Button skillButton3; // 3�� ��ų ��ư�� Reference�� Inspector���� �������ּ���.
    protected WarriorStatus warriorStatus;
    int Plus=1;

    private void Start()
    {
        // skillButton���� Button ������Ʈ�� �����ɴϴ�.
        Button skillButton = GetComponent<Button>();

        // ��ư Ŭ�� �̺�Ʈ�� �����ʸ� �߰��մϴ�.
        skillButton.onClick.AddListener(OnSkillButtonClick);
    }


    private void OnSkillButtonClick()
    {
        warriorStatus.dashVelocity += Plus;       

        // �ٸ� ��ų ��ư�� ��Ȱ��ȭ�մϴ�.
        skillButton.interactable = false;
        skillButton.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.7f);

        skillButton3.interactable = false;
        skillButton3.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.7f);

        // ��ų1 ��ư�� ��Ȱ��ȭ�մϴ�.
        skillButton.interactable = false;
        skillButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}