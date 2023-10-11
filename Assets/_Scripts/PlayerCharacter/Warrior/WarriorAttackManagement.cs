using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackManagement : MonoBehaviour
{
    private enum AttackType { BasicAttack_1, BasicAttack_2, SkillQ, SkillE}
    public AudioClip EnemyHitsoundClip; // 적 피격 사운드 클립
    public AudioSource audioSource; // 오디오 소스 컴포넌트

    private AttackType attackType;
    private float attackRange;

    public WarriorStatus AtkStats;

    public bool isAttacking;

    private void Awake()
    {
        attackType = GetAttackTypeFromName(gameObject.name);
        attackRange = CalculateAttackRange();
    }

    private void Update()
    {
        if (isAttacking)
        {
            isAttacking = false;
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D enemyCollider in hitEnemies)
            {
            if (enemyCollider.CompareTag("Enemy")|| enemyCollider.CompareTag("DeepOne"))
            {
                HandleEnemyCollision(enemyCollider);
                audioSource.PlayOneShot(EnemyHitsoundClip); // Warrior 1-1공격 사운드

            }
            }
            
        }
    }

    private AttackType GetAttackTypeFromName(string name)
    {
        if (name == "BasicAttack_1")
        {
            return AttackType.BasicAttack_1;
        }
        else if (name == "BasicAttack_2")
        {
            return AttackType.BasicAttack_2;
        }
        else if (name == "SkillQ")
        {
            return AttackType.SkillQ;
        }
        else
        {
            return AttackType.SkillE;

        }

    }

    private float CalculateAttackRange()
    {
        float range;
        if (attackType == AttackType.BasicAttack_1)
        {
            range = AtkStats.atkRangeScale;
        }
        else if (attackType == AttackType.BasicAttack_2)
        {
            range = AtkStats.SecondAtkRangeScale;
        }
        else if (attackType == AttackType.SkillQ)
        {
            range = AtkStats.QSkillRangeScale;
        }
        else
        {
            range = AtkStats.ESkillRangeScale;
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
            Debug.Log("뎀지두배 작동");
            damage = AtkStats.power * AtkStats.SecondDamageScale + AtkStats.attackAddness; // 두 번째 공격에는 데미지를 2배로 적용
        }
        if (attackType == AttackType.SkillQ)
        {
            Debug.Log("Q스킬 데미지배율 작동");
            damage = (AtkStats.power + AtkStats.attackAddness) * AtkStats.QSkillDamageScale;
        }
        if (attackType == AttackType.SkillE)
        {
            Debug.Log("E스킬 대미지 배율 작동");
            damage = (AtkStats.power + AtkStats.attackAddness) * AtkStats.QSkillDamageScale;
        }

        if (enemyStatus != null)
        {
            if (attackType == AttackType.SkillQ && AtkStats.SkillTree1 ==1)
            {
                enemyStatus.EnemyStunDamage(0.5 * damage, 3);
                return;
            }
            enemyStatus.TakeDamage(damage);
            Debug.Log(damage);
            if (enemyStatus.currentHealth < 0)
            {
                AtkStats.GainExperience(1);
            }
        }

        if (enemyStatus != null)
        {
            if (attackType == AttackType.SkillE && AtkStats.SkillTree10 == 2)
            {
                enemyStatus.EnemySlow(3);
                return;
            }
            enemyStatus.TakeDamage(damage);
            Debug.Log(damage);
            if (enemyStatus.currentHealth < 0)
            {
                AtkStats.GainExperience(1);
            }
        }
        if (bossStatus != null)
        {
            Debug.Log("보스무력 " + damage);
            bossStatus.TakeNeutralizeGauge(damage);
            if (bossStatus.currentNeutralizeGauge < 0)
            {
                AtkStats.GainExperience(1);
            }
        }
        if (bossHeadHP != null)
        {
            Debug.Log(damage);
            bossHeadHP.TakeBossHeadDamage(damage);
            if (bossHeadHP.BossHeadHealthPower < 0)
            {
                AtkStats.GainExperience(1);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackType == AttackType.BasicAttack_2)
        {
            Gizmos.color = Color.yellow;
        }
        if (attackType == AttackType.SkillQ)
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }



}

