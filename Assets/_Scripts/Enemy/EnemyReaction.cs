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
    public ParticleSystem enemyHitparticleSystem;
    private bool Knock = false;
    private Animator animator;
    public GameObject stunEffect;
    public GameObject stunEffect2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.material.color;
        animator = GetComponent<Animator>();
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
        if (enemyHitparticleSystem != null)
            Instantiate(enemyHitparticleSystem, transform.position, rotation);

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

    public void Stun(float stunDuration)
    {
        stunEffect.SetActive(true); // ���ۺ��� ȿ�� Ȱ��ȭ
        Debug.Log(stunEffect);
        Debug.Log(stunEffect2);
        StartCoroutine(RecoverFromStun(stunDuration));

    }

    private IEnumerator RecoverFromStun(float stunDuration)
    {
        stunEffect2.SetActive(true); // ���ۺ��� ȿ�� Ȱ��ȭ
        yield return new WaitForSeconds(stunDuration);

        stunEffect2.SetActive(false); // ���ۺ��� ȿ�� Ȱ��ȭ
        ReleaseStun();
    }
    private void ReleaseStun()
    {

        animator.SetBool("isEnemyStun", false); // �ִϸ��̼� ����
    }
}


//Movement��  if (!isAttacking && !enemydead || animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_reaction")) ,EnemyStatus�� enemyreaction.Knockback();