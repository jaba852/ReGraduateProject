using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    private int abc = 0;
    public MoveLoading loading;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && abc==0) // �� �ε� ���� �ƴ� ���� ����
        {
            loading.LoadSceneAsync();
            abc++;
        }
    }

    
}
