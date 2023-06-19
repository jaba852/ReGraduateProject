using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    public GameObject[] activatedPrefabs; // ������ ����� �� Ȱ��ȭ�� �������
    public GameObject[] deactivatedPrefabs; // ������ �ٽ� ����� �� ��Ȱ��ȭ�� �������

    private bool isActivated = false; // ��������� Ȱ��ȭ�Ǿ����� ����
    public float activationDelay = 1f; // Ȱ��ȭ ���� �ð�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            // ������ Ȱ��ȭ�� �� ������ ������ ���⿡ �ۼ�
            foreach (GameObject prefab in activatedPrefabs)
            {
                prefab.SetActive(true);
            }

            StartCoroutine(DeactivatePrefabs());
        }
    }

    private IEnumerator DeactivatePrefabs()
    {
        // ���� �ð��� ��ٸ� �� ��Ȱ��ȭ�� ��������� ��ȸ�ϸ� ��Ȱ��ȭ
        yield return new WaitForSeconds(activationDelay);

        foreach (GameObject prefab in deactivatedPrefabs)
        {
            prefab.SetActive(false);
        }

        isActivated = false; // ������ �ٽ� Ȱ��ȭ�� �� �ֵ��� ���� �ʱ�ȭ
    }
}
