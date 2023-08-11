using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public double currentHealth;
    private GameObject transformObject;
    private Animator animatorToChange;
    EnemyReaction enemyreaction;
    private DeepOneMovement DeeponeMvmt;
    private CloseEnemyMovement CloseEnemyMvmt;
    public int expReward = 10; // �� óġ �� �÷��̾ ��� ����ġ��
 


    public void Start()
    {
        currentHealth = maxHealth;
        transformObject = transform.gameObject;
        animatorToChange = transformObject.GetComponent<Animator>();
        enemyreaction = GetComponent<EnemyReaction>();
        DeeponeMvmt = GetComponent<DeepOneMovement>();
        CloseEnemyMvmt = GetComponent<CloseEnemyMovement>();

    }
    public void TakeDamage(double damage)
    {
        Debug.Log("���ǰ�");
        currentHealth -= damage;
        animatorToChange.SetBool("isEnemyHit", true);
        enemyreaction.Knockback();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void EnemyStunDamage(double damage,float stunDuration) 
    {
        Debug.Log("�� ����");
        currentHealth -= damage;
        animatorToChange.SetBool("isEnemyHit", true);
        animatorToChange.SetBool("isEnemyStun", true);
        enemyreaction.Knockback();
        enemyreaction.Stun(stunDuration);
        if (currentHealth <= 0)
        {
            Die();
        }

    }
    private void Die()
    {
        Debug.Log("enemy�ֱ� 10�ʵڼҸ�");
        animatorToChange.SetBool("isEnemyMove", false);
        animatorToChange.SetBool("isEnemyAttack", false);
        animatorToChange.SetBool("isEnemyDead", true);

        // "DeepOne" �±׸� ���� ������Ʈ�� ��� "deadEnemy" �±׷� �����մϴ�.
        if (gameObject.CompareTag("DeepOne"))
        {
            gameObject.tag = "DeadEnemy";
            DeeponeMvmt.SetEnemyDead();
        }

        // "Enemy" �±׸� ���� ������Ʈ�� ��� "deadEnemy" �±׷� �����մϴ�.
        if (gameObject.CompareTag("Enemy"))
        {
            gameObject.tag = "DeadEnemy";
            CloseEnemyMvmt.SetEnemyDead();
        }

        Destroy(transformObject, 10.0f);

        GetComponent<Collider2D>().enabled = false; // Collider2D�� ��Ȱ��ȭ�Ͽ� �浹 ó�� ����

    }



}