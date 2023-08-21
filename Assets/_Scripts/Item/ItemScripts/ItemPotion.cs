using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPotion : MonoBehaviour, IItemFunction
{
    [SerializeField]
    public WarriorStatus status;
    private ItemData item;
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
        Debug.Log(status);
        status.TakeDamagePlayer(-10);
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
        
        item = ItemDatabase.instance.GetItemByID(1);
    }
    private void CheckAndFindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            // playerObject�� ã���� ���� ����
            //Debug.Log(playerObject);
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
