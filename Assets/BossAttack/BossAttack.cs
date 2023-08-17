using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public CircleCollider2D AtkCollider;
    public Animator BossAtkAnim;
    public int BossAtkStep =0;
    private WarriorStatus Atk;
    private void Awake()
    {
        Atk= FindObjectOfType<WarriorStatus>();
        AtkCollider.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(BossAttackCoroutine());
        
    }

    private IEnumerator BossAttackCoroutine()
    {
        BossAtkStep = 1;
        BossAtkAnim.SetInteger("BossAttackStep", BossAtkStep);
        yield return new WaitForSeconds(2f);
        BossAtkStep = 2;
        BossAtkAnim.SetInteger("BossAttackStep", BossAtkStep);
        AtkCollider.isTrigger = true;
        yield return new WaitForSeconds(1f);
        AtkCollider.isTrigger = false;
        BossAtkStep = 0;
        BossAtkAnim.SetInteger("BossAttackStep", BossAtkStep);
        Destroy(gameObject);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Atk.TakeDamagePlayer(20);
        }
    }

    public void enablecollider()
    {
        AtkCollider.enabled = true;
    }
    public void disablecollider()
    {
        AtkCollider.enabled = false;
    }
}
