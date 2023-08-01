using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadHP : MonoBehaviour
{
    public int BosHP = 100;
    public double BossHeadHealthPower;

    public bool isDead;

    public void Awake()
    {
        isDead = false;
    }
    public void Start()
    {
        BossHeadHealthPower = BosHP;

    }

    public void TakeBossHeadDamage(double damage)
    {
        BossHeadHealthPower -= damage;

        Debug.Log("���� �ǰ�" + damage);
        if (BossHeadHealthPower < 0)
        {
            isDead = true;
            Debug.Log("���� ���");
        }
    }
}
