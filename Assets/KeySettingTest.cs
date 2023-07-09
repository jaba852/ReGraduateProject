using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySettingTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Up]))
        {
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Down]))
        {
            Debug.Log("Down");
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Left]))
        {
            Debug.Log("Left");
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Right]))
        {
            Debug.Log("R");
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Dash]))
        {
            Debug.Log("D");
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill1]))
        {
            Debug.Log("S1");
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill2]))
        {
            Debug.Log("S2");
        }
    }
}
