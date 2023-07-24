using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    public int neutralizeGauge = 100;
    public double currentNeutralizeGauge;

    public bool isNeutralize;

    public void Awake()
    {
        isNeutralize = false;
    }
    public void Start()
    {
        currentNeutralizeGauge = neutralizeGauge;

    }

    public void TakeNeutralizeGauge(double damage)
    {
        currentNeutralizeGauge -= damage;

        Debug.Log("보스 피격" + damage);
        if (currentNeutralizeGauge < 0)
        {
            isNeutralize = true;
            Debug.Log("보스 기절");
        }
    }


}
