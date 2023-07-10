using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaction : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform 컴포넌트
    private Rigidbody2D rb;

    public Color hitColor = Color.red;
    public float hitDuration = 0.2f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    private bool Knock=false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.material.color;
        Animator animator = GetComponent<Animator>();
        particleSystem = GameObject.Find("ImpactFlash").GetComponent<ParticleSystem>();
    }
    void FixedUpdate()
    {
        if (!Knock) 
        {
            rb.velocity = Vector2.zero;
        }

    }

    public void Knockback()
    {
        Knock = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector2 direction = (player.position - transform.position).normalized;

        rb.AddForce(-direction * 3f, ForceMode2D.Impulse); //넉백
        UnityEngine.Debug.Log(direction);
        // 스프라이트 색상 변경
        spriteRenderer.material.color = hitColor;
        // 회전값 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // 파티클 생성
        Instantiate(particleSystem, transform.position, rotation);

        // 일정 시간 후에 원래 색상으로 되돌리기
        StartCoroutine(ResetColor());

    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(hitDuration);

        // 원래 색상으로 되돌리기
        spriteRenderer.material.color = originalColor;
        Knock = false;
    }
}


//Movement에  if (!isAttacking && !enemydead || animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_reaction")) ,EnemyStatus에 enemyreaction.Knockback(); 
