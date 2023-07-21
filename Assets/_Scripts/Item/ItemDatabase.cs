using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int itemID;
    public int itemOriginID;
    public Sprite itemImage;
    public string itemName;
    public string itemDescription;
    public int itemGetNumbers;
    public MonoBehaviour itemFunction;
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    public List<ItemData> items = new List<ItemData>();
    private int getItemCount;
    public List<Sprite> ItemsList = new List<Sprite>();
    private ItemInformation itemInformation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (itemInformation == null)
        {
            itemInformation = FindObjectOfType<ItemInformation>();
        }
    }

    private void Start()
    {
        LoadItemData();
        getItemCount = 0;
    }

    private void LoadItemData()
    {
        // 아이템 데이터를 로드하고 items 리스트에 추가하는 로직을 구현합니다.
        // 예를 들어, 리소스에서 데이터를 로드하거나 외부 파일, 데이터베이스 등을 사용할 수 있습니다.

        // 예시: 아이템 데이터 추가
        ItemData item1 = new ItemData();
        item1.itemID = 1;
        item1.itemOriginID = 0;
        item1.itemImage = null;
        item1.itemName = "붉그스름한 포션";
        item1.itemDescription = "끈적한 점성이 느껴진다.";
        item1.itemFunction = gameObject.AddComponent<ItemPotion>();
        item1.itemGetNumbers = 0;
        items.Add(item1);

        // 추가 아이템 데이터 로드 및 할당
        ItemData item2 = new ItemData();
        item2.itemID = 2;
        item2.itemOriginID = 0;
        item2.itemImage = null;
        item2.itemName = "금속몽둥이";
        item2.itemDescription = "미친놈들에겐 이게 약이지";
        item2.itemFunction = gameObject.AddComponent<ItemMace>();
        item2.itemGetNumbers = 0;
        items.Add(item2);

    }

    public ItemData GetItemByID(int id)
    {
        return items.Find(item => item.itemID == id);
    }
    public ItemData GetItemByOriginID(int id)
    {
        return items.Find(item => item.itemOriginID == id);
    }

    public void UseItemByID(int itemID, GameObject itemObject)
    {
        ItemData itemData = GetItemByID(itemID);
        if (itemData != null && itemData.itemFunction != null && itemData.itemFunction is IItemFunction)
        {
            ItemsList.Add(ItemDatabase.instance.GetItemByID(itemID).itemImage);
            getItemCount++;
            IItemFunction itemFunction = (IItemFunction)itemData.itemFunction;
            itemFunction.ItemUsed(itemObject);
        }
    }

    public int GetItemcount()
    {

        return getItemCount;
    }

    public ItemInformation itemInfor()
    {

        return itemInformation;
    }
}


