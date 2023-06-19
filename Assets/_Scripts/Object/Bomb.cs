using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombHealth = 50f;
    public float explosionRadius = 5f;
    public int damage = 30;

    void Update()
    {
        // 폭탄이 체력이 0이 되면 터진다
        if (bombHealth <= 0)
        {
            Explode();
        }
    }

    // 플레이어의 공격에 닿을 때
    void OnCollisionEnter2D(Collision2D other)
    {
        // 충돌한 오브젝트가 플레이어의 공격이라면
        if (other.gameObject.CompareTag("Player"))
        {
            // 폭탄의 체력을 깎는다. 
            
            //if (!WarriorMovement.isAttack == true;){
                
            //}
                bombHealth -= 10f;
        }
    }

    // 폭탄이 터짐
    void Explode()
    {
        // 폭탄 주변에 있는 모든 오브젝트를 가져옴
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in objectsInRange)
        {
            // 오브젝트가 Health 스크립트를 가지고 있는지 확인
            EnemyStatus health = col.GetComponent<EnemyStatus>();
            if (health != null)
            {
                // Health가 있는 모든 오브젝트의 health를 줄인다
                damage = (int)health.currentHealth;
                health.currentHealth = damage - 30;
            }
            WarriorStatus health2 = col.GetComponent<WarriorStatus>();
            if (health2 != null)
            {
                // Health가 있는 모든 오브젝트의 health를 줄인다
                damage = (int)health2.currentHealth;
                health2.currentHealth = damage - 30;
            }
        }

        // 폭탄 오브젝트를 파괴한다
        Destroy(gameObject);
    }
}
