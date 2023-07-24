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
        Debug.Log("적피격");
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
        Debug.Log("적 스턴");
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
        Debug.Log("enemy주금 10초뒤소멸");
        animatorToChange.SetBool("isEnemyMove", false);
        animatorToChange.SetBool("isEnemyAttack", false);
        animatorToChange.SetBool("isEnemyDead", true);
        if (gameObject.CompareTag("DeepOne"))
        {
            DeeponeMvmt.SetEnemyDead();
        }
        if (gameObject.CompareTag("Enemy"))
        {
            CloseEnemyMvmt.SetEnemyDead();
        }


        Destroy(transformObject, 10.0f);

        GetComponent<Collider2D>().enabled = false; // Collider2D를 비활성화하여 충돌 처리 방지

    }



}