using UnityEngine;

using System.Collections;

public class Platform : MonoBehaviour
{
    public GameObject targetObject; // 활성화할 오브젝트

    private bool isActivated = false; // 활성화 여부
    public float activationDelay = 1f; // 활성화 지연 시간

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            if (targetObject != null)
            {
                // 오브젝트의 Light2D 컴포넌트를 가져와서 활성화
                UnityEngine.Rendering.Universal.Light2D light2DComponent = targetObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
                if (light2DComponent != null)
                {
                    light2DComponent.enabled = true;
                }
            }

            StartCoroutine(DeactivatePrefabs());
        }
    }

    private IEnumerator DeactivatePrefabs()
    {
        // 일정 시간을 기다린 후 비활성화할 오브젝트들을 순회하며 비활성화
        yield return new WaitForSeconds(activationDelay);

        if (targetObject != null)
        {
            // 오브젝트의 Light2D 컴포넌트를 가져와서 비활성화
            UnityEngine.Rendering.Universal.Light2D light2DComponent = targetObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light2DComponent != null)
            {
                light2DComponent.enabled = false;
            }
        }

        isActivated = false; // 오브젝트를 다시 활성화할 수 있도록 변수 초기화
    }
}
