 using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveLoading : MonoBehaviour
{
  

    public void LoadSceneAsync()
    {   int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadSceneAsyncCoroutine(sceneBuildIndex));
       
    }

    private IEnumerator LoadSceneAsyncCoroutine(int sceneBuildIndex)
    {    PlayerPrefs.SetInt("sceneIndex",sceneBuildIndex);
        if (sceneBuildIndex == 1) 
        {
            ResetPlayerStats();
        }
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        //SceneManager.LoadSceneAsync(sceneBuildIndex);

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
