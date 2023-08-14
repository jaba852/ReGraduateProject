using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashCoolBoard : MonoBehaviour
{
    public Image dashCoolImage;  // �뽬 ��ٿ� �̹��� ����
    public WarriorMovement warriorMovement;  // �뽬 ��ٿ� ������ �������� ���� ������ �������� �����մϴ�.
    public WarriorStatus stats;

    private bool dashAvailable = true;  // �뽬�� ��� ������ �������� ��Ÿ���� �÷���

    void Start()
    {
        dashCoolImage.fillAmount = 1;  // ó���� ��ٿ� �������� ����
    }

    void Update()
    {

        if (warriorMovement.isCooldownRunning != dashAvailable)
        {
            dashAvailable = warriorMovement.isCooldownRunning;  // �뽬 ��ٿ� ���¸� ������Ʈ
            if (dashAvailable) StartCoroutine(DashCoolDownVisualize());  // ��ٿ� ����
        }

    }

    // ��ٿ��� �����ϴ� ���� ��Ÿ�� �ٸ� �����ϴ� �ڷ�ƾ
    private IEnumerator DashCoolDownVisualize()
    {
        float coolDownTime = stats.DashCoolDown;
        float passedTime = 0;

        while (passedTime < coolDownTime)
        {
            passedTime += Time.deltaTime;
            dashCoolImage.fillAmount = 1 - (passedTime / coolDownTime);  // ���� ��Ÿ�� ������ ��Ÿ�� �ٸ� ����
            yield return null;
        }

        dashCoolImage.fillAmount = 1;  // ��ٿ��� ������ �ٽ� ��ٿ� �̹����� �� ä��
    }
}