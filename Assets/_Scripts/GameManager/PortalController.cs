using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the current scene's build index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Calculate the build index of the next scene by adding 1
            int nextSceneIndex = currentSceneIndex + 1;

            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
