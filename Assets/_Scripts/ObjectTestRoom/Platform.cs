using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    public GameObject[] activatedPrefabs; // 발판을 밟았을 때 활성화할 프리펩들
    public GameObject[] deactivatedPrefabs; // 발판을 다시 밟았을 때 비활성화할 프리펩들

    private bool isActivated = false; // 프리펩들이 활성화되었는지 여부
    public float activationDelay = 1f; // 활성화 지연 시간

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            // 발판이 활성화될 때 실행할 내용을 여기에 작성
            foreach (GameObject prefab in activatedPrefabs)
            {
                prefab.SetActive(true);
            }

            StartCoroutine(DeactivatePrefabs());
        }
    }

    private IEnumerator DeactivatePrefabs()
    {
        // 일정 시간을 기다린 후 비활성화할 프리펩들을 순회하며 비활성화
        yield return new WaitForSeconds(activationDelay);

        foreach (GameObject prefab in deactivatedPrefabs)
        {
            prefab.SetActive(false);
        }

        isActivated = false; // 발판을 다시 활성화할 수 있도록 변수 초기화
    }
}
