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
        MHp.text = "최대체력: " + stats.maxHealth.ToString();
        CHp.text = "현재체력: " + stats.currentHealth.ToString();
        MSpeed.text = "이동속도: " + stats.movementSpeed.ToString();
        VDash.text = "대쉬가속도: " + stats.dashVelocity.ToString();
        TDash.text = "대쉬시간: " + stats.dashingTime.ToString();
        RRAtack.text = "공격범위배수: " + stats.atkRangeRatio.ToString();
        MAttack.text = "공격시이동거리: " + stats.AttackMove.ToString();
        SAttack.text = "공격속도: " + stats.atkSpeed.ToString();
        Power.text = "공격력: " + stats.power.ToString();
        attackAdd.text = "추가데미지: " + stats.attackAddness.ToString();
        invincibleD.text = "무적시간: " + stats.invincibleDuration.ToString();
    }
    
}   
