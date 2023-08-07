using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropBox : MonoBehaviour, IItemFunction
{
    [SerializeField]
    private ItemData item;
    public string prefabFolderPath; // ������ ������ ��θ� Inspector���� �����մϴ�.
    public int maxSpawnCount = 1;   // �����ϰ� ��ȯ�� �ִ� ������ �����մϴ�.


    public void ItemUsed(GameObject itemObject)
    {
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("Item");
        // �����ϰ� �������� ��ȯ�մϴ�.
        int spawnCount = Random.Range(1, maxSpawnCount + 1);
        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, allPrefabs.Length);
            GameObject randomPrefab = allPrefabs[randomIndex];
            Vector3 spawnPosition = itemObject.transform.position;
            Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
        }

        item.itemGetNumbers += 1;
        Destroy(itemObject);
    }
    public void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        item = ItemDatabase.instance.GetItemByID(-1);
    }

}
