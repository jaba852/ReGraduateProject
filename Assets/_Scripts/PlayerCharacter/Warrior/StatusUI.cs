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
        MSpeed.text = "이동속도: " + stats.movementSpeed.ToString();
        SAttack.text = "공격속도: " + stats.atkSpeed.ToString();
        Power.text = "공격력: " + stats.power.ToString();
        attackAdd.text = "추가데미지: " + stats.attackAddness.ToString();
        DashStack.text = "구르기 최대 스택: " + stats.dashStack.ToString();
        DashCoolDown.text = "구르기 쿨타임: " + stats.DashCoolDown.ToString();
        QCoolDown.text = "Q스킬 쿨타임: " + stats.QCoolDown.ToString();
        ECoolDown.text = "E스킬 쿨타임: " + stats.ECoolDown.ToString();
    }
    
}   
