using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGeneratorLoader : MonoBehaviour
{
    public void LoadMapGeneratorScene()
    {
        SceneManager.LoadScene("MapGenerator", LoadSceneMode.Additive);
    }
}
