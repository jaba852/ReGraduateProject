using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaction : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform ������Ʈ
    private Rigidbody2D rb;

    public Color hitColor = Color.red;
    public float hitDuration = 0.2f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem ImpactFlash;
    private bool Knock=false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.material.color;
        Animator animator = GetComponent<Animator>();
        ImpactFlash = GameObject.Find("ImpactFlash").GetComponent<ParticleSystem>();
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

        rb.AddForce(-direction * 3f, ForceMode2D.Impulse); //�˹�
        UnityEngine.Debug.Log(direction);
        // ��������Ʈ ���� ����
        spriteRenderer.material.color = hitColor;
        // ȸ���� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // ��ƼŬ ����
        Instantiate(ImpactFlash, transform.position, rotation);

        // ���� �ð� �Ŀ� ���� �������� �ǵ�����
        StartCoroutine(ResetColor());

    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(hitDuration);

        // ���� �������� �ǵ�����
        spriteRenderer.material.color = originalColor;
        Knock = false;
    }
}


//Movement��  if (!isAttacking && !enemydead || animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_reaction")) ,EnemyStatus�� enemyreaction.Knockback(); 
