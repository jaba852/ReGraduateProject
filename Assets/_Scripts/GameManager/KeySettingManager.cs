using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum KeyAction { Up, Down, Left, Right, Dash, Skill1, Skill2, KeyCount }

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeySettingManager : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space, KeyCode.Q, KeyCode.E };

    private void Awake()
    {
        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            if (!KeySetting.keys.ContainsKey((KeyAction)i))
            {
                KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
            }
        }
    }
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey && key != -1)
        {
            // 현재 입력된 키를 가져옴
            KeyCode newKey = keyEvent.keyCode;

            // 중복된 키가 있는지 확인
            bool isDuplicate = false;
            foreach (var pair in KeySetting.keys)
            {
                if (pair.Value == newKey && pair.Key != (KeyAction)key)
                {
                    isDuplicate = true;
                    break;
                }
            }

            // 중복된 키가 있다면 경고 창을 띄우고 기존 키로 유지
            if (isDuplicate)
            {

            }
            else
            {
                // 중복된 키가 없으면 새로운 키로 변경
                KeySetting.keys[(KeyAction)key] = newKey;
            }

            key = -1;
        }
    }

    int key = -1;

    public void ChangeKey(int num)
    {
        key = num;
    }
}
