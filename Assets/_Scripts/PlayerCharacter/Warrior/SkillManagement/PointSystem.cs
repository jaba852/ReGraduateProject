using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static PointSystem Instance;

    // ���� ����Ʈ
    private int currentPoints = 20;

    // CurrentPoints �Ӽ�

    public void SetPoints(int newPoints)
    {
        currentPoints = newPoints;
    }

    public int CurrentPoints
    {
        get { return currentPoints; }
        set { currentPoints = value; } // ���⿡ set �����ڸ� �߰��մϴ�.
    }


    void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoint(int point)
    {
        currentPoints += point;
        Debug.Log("���� ����Ʈ: " + currentPoints);
    }
}
