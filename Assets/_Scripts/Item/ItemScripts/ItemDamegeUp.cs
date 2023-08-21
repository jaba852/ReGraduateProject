using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemDamegeUp : MonoBehaviour, IItemFunction
{
    // Start is called before the first frame update
    [SerializeField]
    public WarriorStatus status;
    private ItemData item;
    public int DamegeUp = 5;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start(); // Start 메서드 내용을 여기서 호출

    }

    public void ItemUsed(GameObject itemObject)
    {

        item.itemGetNumbers += 1;
        status.power+= DamegeUp;// 데미지 증가
        Destroy(itemObject);
    }
    public void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
 
        if (playerObject == null)
        {
            InvokeRepeating("CheckAndFindPlayer", 1.0f, 1.0f);
        }
        if (playerObject != null)
        { 
            status = playerObject.GetComponent<WarriorStatus>();
        }
        
        item = ItemDatabase.instance.GetItemByID(3);
    }
    private void CheckAndFindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            // playerObject를 찾았을 때의 동작
            //Debug.Log("Player object found.");
            status = playerObject.GetComponent<WarriorStatus>();
            // 찾았으면 InvokeRepeating 중단
            CancelInvoke("CheckAndFindPlayer");
        }
        else
        {
            // playerObject가 아직 null인 경우의 동작
            //Debug.Log("Player object not found yet.");
        }
    }
}
