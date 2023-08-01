using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandAttack : MonoBehaviour
{
    private WarriorStatus warriorStatus;

    private void Awake()
    {
        warriorStatus = FindObjectOfType<WarriorStatus>();

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            warriorStatus.TakeDamagePlayer(10);
        }
    }
}
