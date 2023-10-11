using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnterTrigger : MonoBehaviour
{
    public bool hasInteracted = false; // 상호작용 여부를 추적하기 위한 변수
    public GameObject objectToActivate; // 활성화할 오브젝트를 할당할 변수
    public BossCamera bossCameraInstance;

    private void Start()
    {
        objectToActivate.SetActive(false);

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("asd");

        if (other.CompareTag("Player") && !hasInteracted)
        {
            DoInteraction();
            hasInteracted = true;
            bossCameraInstance.FixCamera();
        }
    }

    private void DoInteraction()
    {
        gameObject.SetActive(false);

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
