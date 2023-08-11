using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ESkillCoolBoard : MonoBehaviour
{
    public Image skillCoolImage; // E ��ų ��ٿ� �̹��� ����
    public WarriorMovement warriorMovement; // �뽬 ��ٿ� ������ �������� ���� ������ �������� �����մϴ�.
    public WarriorStatus stats;

    private bool skillEAvailable = true; // E ��ų�� ��� ������ �������� ��Ÿ���� �÷���

    void Start()
    {
        skillCoolImage.fillAmount = 1; // ó���� ��ٿ� �������� ����
    }

    void Update()
    {
        if (warriorMovement.canUseSkillE != skillEAvailable)
        {
            skillEAvailable = warriorMovement.canUseSkillE; // E ��ų ��� ���� ���¸� ������Ʈ
            if (!skillEAvailable) StartCoroutine(SkillCoolDownVisualize(stats.ECoolDown)); // ��ٿ� ����
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
