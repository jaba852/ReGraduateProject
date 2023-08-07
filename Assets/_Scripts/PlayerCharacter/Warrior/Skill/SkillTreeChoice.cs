using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeChoice : MonoBehaviour
{
    public Button[] buttons;
    public RawImage[] images;

    public WarriorStatus warriorStatus;

    public int skillTreeIndex;

    private int currentState;

    private void Start()
    {
        SetUIState(currentState);
    }

    private void Update()
    {
        int newState = GetSkillTreeState();
        if (currentState != newState)
        {
            currentState = newState;
            SetUIState(currentState);
        }
    }

    private int GetSkillTreeState()
    {
        switch (skillTreeIndex)
        {
            case 1:
                return warriorStatus.SkillTree1;
            case 3:
                return warriorStatus.SkillTree3;
            case 5:
                return warriorStatus.SkillTree5;
            case 10:
                return warriorStatus.SkillTree10;
            case 13:
                return warriorStatus.SkillTree13;
            case 15:
                return warriorStatus.SkillTree15;
            case 18:
                return warriorStatus.SkillTree18;
            case 20:
                return warriorStatus.SkillTree20;
            default:
                return 0;
        }
    }

    private void SetUIState(int state)
    {
        if (state == 0)
        {
            buttons[0].interactable = true;
            images[0].color = new Color(1f, 1f, 1f, 1f);
            buttons[1].interactable = true;
            images[1].color = new Color(1f, 1f, 1f, 1f);
            buttons[2].interactable = true;
            images[2].color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
                images[i].color = new Color(1f, 1f, 1f, (state == i + 1) ? 1f : 0.5f);
            }
        }
       
    }

    public void ButtonChoose(int newState)
    {
        switch (skillTreeIndex)
        {
            case 1:
                warriorStatus.SkillTree1 = newState;
                break;
            case 3:
                warriorStatus.SkillTree3 = newState;
                break;
            case 5:
                warriorStatus.SkillTree5 = newState;
                break;
            case 7:
                warriorStatus.SkillTree7 = newState;
                break;
            case 10:
                warriorStatus.SkillTree10 = newState;
                break;
            case 13:
                warriorStatus.SkillTree13 = newState;
                break;
            case 15:
                warriorStatus.SkillTree15 = newState;
                break;
            case 18:
                warriorStatus.SkillTree18 = newState;
                break;
            case 20:
                warriorStatus.SkillTree20 = newState;
                break;
            default:
                break;
        }
        SetUIState(newState);

    }
    public void OnButtonPointerDown(int buttonIndex)
    {
        buttons[buttonIndex].image.color = Color.black;
    }

    public void OnButtonPointerUp(int buttonIndex)
    {
        buttons[buttonIndex].image.color = Color.white;
    }
}
