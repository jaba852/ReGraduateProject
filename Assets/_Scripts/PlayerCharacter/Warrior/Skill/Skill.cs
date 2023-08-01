using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Button skillButton; // ��ư ������Ʈ ������ ���� ���� ����
    public Image[] otherSkillsToDisable; // ��Ȱ��ȭ�� �ٸ� ��ų�鿡 ���� ����
    public float skillCost = 1; // ��ų ��� ���

    protected WarriorStatus warriorStatus;
    protected PointSystem pointSystem;

    protected virtual void Start()
    {
        warriorStatus = FindObjectOfType<WarriorStatus>(); // ���� ���¿� ���� ���� ȹ��
        //pointSystem = PointSystem.Instance; // ����Ʈ �ý��� �ν��Ͻ� ȹ��

        skillButton.onClick.AddListener(UseSkill); // ��ų ����� ��ư Ŭ�� �̺�Ʈ �����ʿ� �߰�
    }

    public void UseSkill()
    {
        if (pointSystem.CurrentPoints >= skillCost) // ���� ����Ʈ�� ��ų ��뺸�� ���ų� ���� ���
        {
            ApplyEffect(); // ��ų ȿ�� ����
            pointSystem.CurrentPoints -= Mathf.FloorToInt(skillCost); // ��ų ��븸ŭ ����Ʈ ����

            //foreach (Image skill in otherSkillsToDisable) // �ٸ� ��ų���� ��Ȱ��ȭ
           // {
           //     skill.enabled = false;
           // }

            skillCost *= 1.2f; // ��ų ��� 20% ����
        }
    }

    protected virtual void ApplyEffect()
    {
        // �� �޼���� �ڽ� Ŭ�������� �������̵�˴ϴ�.
    }
}
