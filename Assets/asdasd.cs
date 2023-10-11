using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdasd : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("asdasdasdasda");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("222222");
        }
    }
}
