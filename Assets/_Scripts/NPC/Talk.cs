using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class Talk : MonoBehaviour
{
    public GameManager Manager;
    // Start is called before the first frame update
    private void Update()
    {

            RaycastHit2D hit = Physics2D.CircleCast(transform.position,1.5f,Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("¿€µø1");
                Manager.TalkAction(gameObject);
            }
        }
    }
}
