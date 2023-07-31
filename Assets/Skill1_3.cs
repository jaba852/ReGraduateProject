using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill1_3 : MonoBehaviour
{
    public Button skillButton; // 1번 스킬 버튼의 Reference를 Inspector에서 연결해주세요.
    public Button skillButton2; // 2번 스킬 버튼의 Reference를 Inspector에서 연결해주세요.
    public Button skillButton3; // 3번 스킬 버튼의 Reference를 Inspector에서 연결해주세요.
    protected WarriorStatus warriorStatus;
    int Plus=1;

    private void Start()
    {
        // skillButton에서 Button 컴포넌트를 가져옵니다.
        Button skillButton = GetComponent<Button>();

        // 버튼 클릭 이벤트에 리스너를 추가합니다.
        skillButton.onClick.AddListener(OnSkillButtonClick);
    }


    private void OnSkillButtonClick()
    {
        warriorStatus.dashVelocity += Plus;       

        // 다른 스킬 버튼을 비활성화합니다.
        skillButton.interactable = false;
        skillButton.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.7f);

        skillButton3.interactable = false;
        skillButton3.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.7f);

        // 스킬1 버튼을 비활성화합니다.
        skillButton.interactable = false;
        skillButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}