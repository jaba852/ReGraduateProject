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
        // ������ �����͸� �ε��ϰ� items ����Ʈ�� �߰��ϴ� ������ �����մϴ�.
        // ���� ���, ���ҽ����� �����͸� �ε��ϰų� �ܺ� ����, �����ͺ��̽� ���� ����� �� �ֽ��ϴ�.

        // ����: ������ ������ �߰�
        ItemData item1 = new ItemData();
        item1.itemID = 1;
        item1.itemOriginID = 0;
        item1.itemImage = null;
        item1.itemName = "�ӱ׽����� ����";
        item1.itemDescription = "������ ������ ��������.";
        item1.itemFunction = gameObject.AddComponent<ItemPotion>();
        item1.itemGetNumbers = 0;
        items.Add(item1);

        // �߰� ������ ������ �ε� �� �Ҵ�
        ItemData item2 = new ItemData();
        item2.itemID = 2;
        item2.itemOriginID = 0;
        item2.itemImage = null;
        item2.itemName = "�ݼӸ�����";
        item2.itemDescription = "��ģ��鿡�� �̰� ������";
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


