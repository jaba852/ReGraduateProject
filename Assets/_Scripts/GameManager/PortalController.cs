using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    private int abc = 0;
    public MoveLoading loading;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && abc==0) // 씬 로딩 중이 아닐 때만 실행
        {
            loading.LoadSceneAsync();
            abc++;
        }
    }

    
}
