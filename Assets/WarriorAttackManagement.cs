using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackManagement : MonoBehaviour
{
    private enum AttackType { BasicAttack_1, BasicAttack_2 }

    private AttackType attackType;
    private float attackRange;

    public WarriorStatus AtkStats;

    private void Awake()
    {
        attackType = GetAttackTypeFromName(gameObject.name);
        attackRange = CalculateAttackRange();
    }

    private void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                HandleEnemyCollision(enemyCollider);
            }
        }
    }

    private AttackType GetAttackTypeFromName(string name)
    {
        if (name == "BasicAttack_1")
        {
            return AttackType.BasicAttack_1;
        }
        else
        {
            return AttackType.BasicAttack_2;
        }
    }

    private float CalculateAttackRange()
    {
        float range;
        if (attackType == AttackType.BasicAttack_1)
        {
            range = AtkStats.atkRangeRatio;
        }
        else
        {
            range = AtkStats.atkRangeRatio * 1.5f;
        }
        return range;
    }

    private void HandleEnemyCollision(Collider2D enemyCollider)
    {
        EnemyStatus enemyStatus = enemyCollider.GetComponent<EnemyStatus>();
        BossStatus bossStatus = enemyCollider.GetComponent<BossStatus>();
        BossHeadHP bossHeadHP = enemyCollider.GetComponent<BossHeadHP>();

        float damage = AtkStats.power + AtkStats.attackAddness;

        if (attackType == AttackType.BasicAttack_2)
        {
            damage *= 2f; // 두 번째 공격에는 데미지를 2배로 적용
        }

        if (enemyStatus != null)
        {
            enemyStatus.TakeDamage(damage);
        }
        if (bossStatus != null)
        {
            bossStatus.TakeNeutralizeGauge(damage);
        }
        if (bossHeadHP != null)
        {
            bossHeadHP.TakeBossHeadDamage(damage);
        }
        StartCoroutine(isAttackingD());

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackType == AttackType.BasicAttack_2)
        {
            Gizmos.color = Color.yellow;
        }
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private IEnumerator isAttackingD()  // 한번에 여러번 공격하는걸 방지하기위한 코루틴
    {
        yield return new WaitForSeconds(1f / AtkStats.atkSpeed);
    }


}

