using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

    GameObject playerObject;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    
    }
  
}
