using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkUp : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public float particleDuration = 2f; // 파티클 지속 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WarriorStatus warriorStatus = collision.GetComponent<WarriorStatus>();
            if (warriorStatus != null)
            {
                warriorStatus.power += 1; // 공격력 1 증가

                if (particleEffect != null)
                {
                    particleEffect.Play(); // 파티클 이펙트 재생
                    StartCoroutine(StopParticleEffect());
                }

                Destroy(gameObject);
            }
        }
    }

    private IEnumerator StopParticleEffect()
    {
        yield return new WaitForSeconds(particleDuration); // 일정 시간 동안 대기

        if (particleEffect != null)
        {
            particleEffect.Stop(); // 파티클 이펙트 멈춤
        }
    }
}
