using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{   
    public Image progressfill;
    void Start()
    {
        if(PlayerPrefs.GetInt("sceneIndex")!= null)
        {
        int sceneBuildIndex = PlayerPrefs.GetInt("sceneIndex") + 1;
        
        StartCoroutine(LoadSceneAsyncCoroutine(sceneBuildIndex));
        }
        else 
        SceneManager.LoadSceneAsync(1);
    }


    private IEnumerator LoadSceneAsyncCoroutine(int sceneBuildIndex)
    {   yield return null;
        if (sceneBuildIndex == 1) 
        {
            ResetPlayerStats();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);
        asyncLoad.allowSceneActivation = false;

        float timer = 0.0f;

        while (!asyncLoad.isDone)
        {    yield return null;
             timer += Time.deltaTime;
             if(asyncLoad.progress<0.9f){
             progressfill.fillAmount =  Mathf.Lerp(progressfill.fillAmount,  asyncLoad.progress, timer);
                if(progressfill.fillAmount >=asyncLoad.progress){
                timer = 0f;
                }}   
             else 
            {
                progressfill.fillAmount = Mathf.Lerp(progressfill.fillAmount,1f,timer);
                if(progressfill.fillAmount == 1.0f){
                asyncLoad.allowSceneActivation = true;
                 PlayerPrefs.SetInt("sceneIndex",0);
                yield break;
                }
            }
            
        
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
