using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PointSystem Instance;

    // 현재 포인트
    private int currentPoints = 20;

    // CurrentPoints 속성

    public void SetPoints(int newPoints)
    {
        currentPoints = newPoints;
    }

    public int CurrentPoints
    {
        get { return currentPoints; }
        set { currentPoints = value; } // 여기에 set 접근자를 추가합니다.
    }


    void Awake()
    {
        // 싱글톤 패턴
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
        Debug.Log("현재 포인트: " + currentPoints);
    }
}
