using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnterTrigger : MonoBehaviour
{
    public bool hasInteracted = false; // ��ȣ�ۿ� ���θ� �����ϱ� ���� ����
    public GameObject objectToActivate; // Ȱ��ȭ�� ������Ʈ�� �Ҵ��� ����
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
