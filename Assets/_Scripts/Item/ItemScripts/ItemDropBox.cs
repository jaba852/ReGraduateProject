using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropBox : MonoBehaviour, IItemFunction
{
    [SerializeField]
    private ItemData item;
    public string prefabFolderPath; // 프리펩 폴더의 경로를 Inspector에서 설정합니다.
    public int maxSpawnCount = 1;   // 랜덤하게 소환할 최대 개수를 설정합니다.


    public void ItemUsed(GameObject itemObject)
    {
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("Item");
        // 랜덤하게 프리펩을 소환합니다.
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
