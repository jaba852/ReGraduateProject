using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUp : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public float particleDuration = 2f; // ��ƼŬ ���� �ð�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WarriorStatus warriorStatus = collision.GetComponent<WarriorStatus>();
            if (warriorStatus != null)
            {
                warriorStatus.atkSpeed += 1; // ���ݼӵ� 1 ����

                if (particleEffect != null)
                {
                    particleEffect.Play(); // ��ƼŬ ����Ʈ ���
                    StartCoroutine(StopParticleEffect());
                }

                Destroy(gameObject);
            }
        }
    }

    private IEnumerator StopParticleEffect()
    {
        yield return new WaitForSeconds(particleDuration); // ���� �ð� ���� ���

        if (particleEffect != null)
        {
            particleEffect.Stop(); // ��ƼŬ ����Ʈ ����
        }
    }
}
