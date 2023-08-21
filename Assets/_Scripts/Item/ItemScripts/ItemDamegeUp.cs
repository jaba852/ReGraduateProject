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
        Start(); // Start �޼��� ������ ���⼭ ȣ��

    }

    public void ItemUsed(GameObject itemObject)
    {

        item.itemGetNumbers += 1;
        status.power+= DamegeUp;// ������ ����
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
            // playerObject�� ã���� ���� ����
            //Debug.Log("Player object found.");
            status = playerObject.GetComponent<WarriorStatus>();
            // ã������ InvokeRepeating �ߴ�
            CancelInvoke("CheckAndFindPlayer");
        }
        else
        {
            // playerObject�� ���� null�� ����� ����
            //Debug.Log("Player object not found yet.");
        }
    }
}
