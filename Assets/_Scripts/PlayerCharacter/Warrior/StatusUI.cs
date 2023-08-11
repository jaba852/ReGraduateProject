using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public WarriorStatus stats;
    public Text MSpeed;
    public Text SAttack;
    public Text Power;
    public Text attackAdd;
    public Text DashStack;
    public Text DashCoolDown;
    public Text QCoolDown;
    public Text ECoolDown;
    public GameObject canvas;
    public GameObject SkillCanvas;

    private void Start()
    {
        canvas.SetActive(false);
        SkillCanvas.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvas.SetActive(!canvas.activeSelf);
            SkillCanvas.SetActive(!SkillCanvas.activeSelf);
        }
        MSpeed.text = "�̵��ӵ�: " + stats.movementSpeed.ToString();
        SAttack.text = "���ݼӵ�: " + stats.atkSpeed.ToString();
        Power.text = "���ݷ�: " + stats.power.ToString();
        attackAdd.text = "�߰�������: " + stats.attackAddness.ToString();
        DashStack.text = "������ �ִ� ����: " + stats.dashStack.ToString();
        DashCoolDown.text = "������ ��Ÿ��: " + stats.DashCoolDown.ToString();
        QCoolDown.text = "Q��ų ��Ÿ��: " + stats.QCoolDown.ToString();
        ECoolDown.text = "E��ų ��Ÿ��: " + stats.ECoolDown.ToString();
    }
    
}   
