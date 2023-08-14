using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QSkillCoolBoard : MonoBehaviour
{
    public Image skillCoolImage; // Q ��ų ��ٿ� �̹��� ����
    public WarriorMovement warriorMovement;
    public WarriorStatus stats;

    private bool skillQAvailable = true; // Q ��ų�� ��� ������ �������� ��Ÿ���� �÷���

    void Start()
    {
        skillCoolImage.fillAmount = 1; // ó���� ��ٿ� �������� ����
    }

    void Update()
    {
        if (warriorMovement.canUseSkillQ != skillQAvailable)
        {
            skillQAvailable = warriorMovement.canUseSkillQ; // Q ��ų ��� ���� ���¸� ������Ʈ
            if (!skillQAvailable) StartCoroutine(SkillCoolDownVisualize(stats.QCoolDown)); // ��ٿ� ����
        }
    }

    // ��ٿ��� �����ϴ� ���� ��Ÿ�� �ٸ� �����ϴ� �ڷ�ƾ
    private IEnumerator SkillCoolDownVisualize(float coolDownTime)
    {
        float passedTime = 0;

        while (passedTime < coolDownTime)
        {
            passedTime += Time.deltaTime;
            skillCoolImage.fillAmount = 1 - (passedTime / coolDownTime); // ���� ��Ÿ�� ������ ��Ÿ�� �ٸ� ����
            yield return null;
        }

        skillCoolImage.fillAmount = 1; // ��ٿ��� ������ �ٽ� ��ٿ� �̹����� �� ä��
    }
}
