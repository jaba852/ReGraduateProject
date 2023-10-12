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
            // ���� �Էµ� Ű�� ������
            KeyCode newKey = keyEvent.keyCode;

            // �ߺ��� Ű�� �ִ��� Ȯ��
            bool isDuplicate = false;
            foreach (var pair in KeySetting.keys)
            {
                if (pair.Value == newKey && pair.Key != (KeyAction)key)
                {
                    isDuplicate = true;
                    break;
                }
            }

            // �ߺ��� Ű�� �ִٸ� ��� â�� ���� ���� Ű�� ����
            if (isDuplicate)
            {

            }
            else
            {
                // �ߺ��� Ű�� ������ ���ο� Ű�� ����
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
