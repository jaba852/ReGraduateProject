using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepOneEnergyPoint : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private WarriorStatus warriorStatus;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        warriorStatus = FindObjectOfType<WarriorStatus>();
    }

    public void EnergyBoob()
    {
        StartCoroutine(ActivateColliderForOneSecond());
    }

    private IEnumerator ActivateColliderForOneSecond()
    {
        circleCollider.enabled = true; // Circle Collider 2D Ȱ��ȭ

        yield return new WaitForSeconds(1f); // 1�� ���

        circleCollider.enabled = false; // Circle Collider 2D ��Ȱ��ȭ
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            warriorStatus.TakeDamagePlayer(10);
            UnityEngine.Debug.Log("DeepOne ������");
        }
    }
}
