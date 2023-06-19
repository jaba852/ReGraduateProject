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
        Debug.Log("���ǰ�");
        currentHealth -= damage;
        animatorToChange.SetBool("isEnemyHit", true);
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
        attackManager.Enemydead(parentObject);


        GetComponent<Collider2D>().enabled = false; // Collider2D�� ��Ȱ��ȭ�Ͽ� �浹 ó�� ����
        StartCoroutine(RemoveBody());
    }


    private IEnumerator RemoveBody()
    {
        yield return new WaitForSeconds(1f);



    }
}