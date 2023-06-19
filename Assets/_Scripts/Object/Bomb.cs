using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombHealth = 50f;
    public float explosionRadius = 5f;
    public int damage = 30;

    void Update()
    {
        // ��ź�� ü���� 0�� �Ǹ� ������
        if (bombHealth <= 0)
        {
            Explode();
        }
    }

    // �÷��̾��� ���ݿ� ���� ��
    void OnCollisionEnter2D(Collision2D other)
    {
        // �浹�� ������Ʈ�� �÷��̾��� �����̶��
        if (other.gameObject.CompareTag("Player"))
        {
            // ��ź�� ü���� ��´�. 
            
            //if (!WarriorMovement.isAttack == true;){
                
            //}
                bombHealth -= 10f;
        }
    }

    // ��ź�� ����
    void Explode()
    {
        // ��ź �ֺ��� �ִ� ��� ������Ʈ�� ������
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in objectsInRange)
        {
            // ������Ʈ�� Health ��ũ��Ʈ�� ������ �ִ��� Ȯ��
            EnemyStatus health = col.GetComponent<EnemyStatus>();
            if (health != null)
            {
                // Health�� �ִ� ��� ������Ʈ�� health�� ���δ�
                damage = (int)health.currentHealth;
                health.currentHealth = damage - 30;
            }
            WarriorStatus health2 = col.GetComponent<WarriorStatus>();
            if (health2 != null)
            {
                // Health�� �ִ� ��� ������Ʈ�� health�� ���δ�
                damage = (int)health2.currentHealth;
                health2.currentHealth = damage - 30;
            }
        }

        // ��ź ������Ʈ�� �ı��Ѵ�
        Destroy(gameObject);
    }
}
