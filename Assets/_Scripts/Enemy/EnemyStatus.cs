using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public double currentHealth;
    public GameObject parentObject;
    public Animator animatorToChange;
    private EnemyAttackManager attackManager;


    public void Start()
    {
        currentHealth = maxHealth;
        GameObject parentObject = transform.parent.gameObject;
        animatorToChange = parentObject.GetComponent<Animator>();
        attackManager = FindObjectOfType<EnemyAttackManager>();


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
        attackManager.Enemydead(parentObject);


        GetComponent<Collider2D>().enabled = false; // Collider2D를 비활성화하여 충돌 처리 방지
        StartCoroutine(RemoveBody());
    }


    private IEnumerator RemoveBody()
    {
        yield return new WaitForSeconds(1f);



    }
}