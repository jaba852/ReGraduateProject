using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public CircleCollider2D AtkCollider;
    public Animator BossAtkAnim;
    public int BossAtkStep =0;

    private void Awake()
    {
        BossAtkAnim = GetComponent<Animator>();
        AtkCollider = GetComponent<CircleCollider2D>();
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
}
