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

        DeeponeMvmt.SetEnemyDead();

        Destroy(transformObject, 10.0f);

        GetComponent<Collider2D>().enabled = false; // Collider2D�� ��Ȱ��ȭ�Ͽ� �浹 ó�� ����

    }



}