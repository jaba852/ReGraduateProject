using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    private bool isLoadingScene = false; // 씬 로딩 중인지 여부를 나타내는 플래그

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isLoadingScene) // 씬 로딩 중이 아닐 때만 실행
        {
            // Get the current scene's build index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Calculate the build index of the next scene by adding 1
            int nextSceneIndex = currentSceneIndex + 1;

            // Start loading the next scene asynchronously
            StartCoroutine(LoadSceneAsync(nextSceneIndex));
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        isLoadingScene = true; // 씬 로딩 시작

        // Create an async operation to load the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Prevent the scene from automatically activating on load
        asyncLoad.allowSceneActivation = false;

        // Show loading screen, progress bar, or any other UI elements here

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            // Update progress bar, loading text, or any other UI elements here

            // Check if the loading progress has reached 0.9 (90%)
            if (asyncLoad.progress >= 0.9f)
            {
                // Allow the scene to activate
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoadingScene = false; // 씬 로딩 완료
    }
}
