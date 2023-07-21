using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBIg : MonoBehaviour
{
    public CircleCollider2D AtkCollider;
    public Animator BossAtkAnim;
    public int BossAtkStepB = 0;

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
        BossAtkStepB = 1;
        BossAtkAnim.SetInteger("BossAttackStep", BossAtkStepB);
        yield return new WaitForSeconds(2f);
        BossAtkStepB = 2;
        BossAtkAnim.SetInteger("BossAttackStep", BossAtkStepB);
        AtkCollider.isTrigger = true;
        yield return new WaitForSeconds(1f);
        AtkCollider.isTrigger = false;
        BossAtkStepB = 0;
        BossAtkAnim.SetInteger("BossAttackStep", BossAtkStepB);
        Destroy(gameObject);

    }
}
