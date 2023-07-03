using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public double currentHealth;
    private GameObject transformObject;
    private Animator animatorToChange;
    private DeepOneAttackManager attackManager;
    private DeepOneMovement DeeponeMvmt;
    


    public void Start()
    {
        currentHealth = maxHealth;
        transformObject = transform.gameObject;
        animatorToChange = transformObject.GetComponent<Animator>();
        attackManager = FindObjectOfType<DeepOneAttackManager>();
        DeeponeMvmt = GetComponent<DeepOneMovement>();

    }
    public void TakeDamage(double damage)
    {
        Debug.Log("적피격");
        currentHealth -= damage;
        animatorToChange.SetBool("isEnemyHit", true);
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

        DeeponeMvmt.SetEnemyDead();

        Destroy(transformObject, 10.0f);

        GetComponent<Collider2D>().enabled = false; // Collider2D를 비활성화하여 충돌 처리 방지

    }



}