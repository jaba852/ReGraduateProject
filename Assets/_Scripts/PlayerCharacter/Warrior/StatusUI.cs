using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public WarriorStatus stats;
    public Text MHp;
    public Text CHp;
    public Text MSpeed;
    public Text VDash;
    public Text TDash;
    public Text RRAtack;
    public Text MAttack;
    public Text SAttack;
    public Text Power;
    public Text attackAdd;
    public Text invincibleD;
    public GameObject canvas;

    private void Start()
    {

        stats = GetComponent<WarriorStatus>();
        MHp = GameObject.Find("MaxHp").GetComponent<Text>();
        CHp = GameObject.Find("CurrentHp").GetComponent<Text>();
        MSpeed = GameObject.Find("MoveSpeed").GetComponent<Text>();
        VDash = GameObject.Find("DashVelocity").GetComponent<Text>();
        TDash = GameObject.Find("DashTime").GetComponent<Text>();
        RRAtack = GameObject.Find("AttackRangeRatio").GetComponent<Text>();
        MAttack = GameObject.Find("MoveAttack").GetComponent<Text>();
        SAttack = GameObject.Find("AttackSpeed").GetComponent<Text>();
        Power = GameObject.Find("Power").GetComponent<Text>();
        attackAdd= GameObject.Find("AttackAdd").GetComponent<Text>();
        invincibleD = GameObject.Find("InvincibleDuration").GetComponent<Text>();

        canvas.SetActive(false);
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
        MHp.text = "�ִ�ü��: " + stats.maxHealth.ToString();
        CHp.text = "����ü��: " + stats.currentHealth.ToString();
        MSpeed.text = "�̵��ӵ�: " + stats.movementSpeed.ToString();
        VDash.text = "�뽬���ӵ�: " + stats.dashVelocity.ToString();
        TDash.text = "�뽬�ð�: " + stats.dashingTime.ToString();
        RRAtack.text = "���ݹ������: " + stats.atkRangeRatio.ToString();
        MAttack.text = "���ݽ��̵��Ÿ�: " + stats.AttackMove.ToString();
        SAttack.text = "���ݼӵ�: " + stats.atkSpeed.ToString();
        Power.text = "���ݷ�: " + stats.power.ToString();
        attackAdd.text = "�߰�������: " + stats.attackAddness.ToString();
        invincibleD.text = "�����ð�: " + stats.invincibleDuration.ToString();
    }
    
}   
