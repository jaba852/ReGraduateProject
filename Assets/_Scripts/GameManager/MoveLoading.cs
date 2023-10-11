 using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveLoading : MonoBehaviour
{

    public static int sceneNum = 0;

    public void LoadSceneAsync()
    {   
        sceneNum = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadSceneAsyncCoroutine(sceneNum));
       
    }

    private IEnumerator LoadSceneAsyncCoroutine(int num)
    {    
        PlayerPrefs.SetInt("sceneIndex", num);
        if (num == 1) 
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
