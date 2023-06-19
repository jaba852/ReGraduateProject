using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    public void LoadSceneAsync(int sceneBuildIndex)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneBuildIndex));
    }

    private IEnumerator LoadSceneAsyncCoroutine(int sceneBuildIndex)
    {
        if (sceneBuildIndex == 1) 
        {
            ResetPlayerStats();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ResetPlayerStats()
    {
        WarriorStatus warriorStatus = FindObjectOfType<WarriorStatus>();
        if (warriorStatus != null)
        {
            warriorStatus.InitializePlayerStats();
        }
    }
}
