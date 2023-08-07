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
    public List<int> itemindex = new List<int>();
    private ItemInformation itemInformation;
    private ItemTooltip itemTooltip;

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
        if(itemTooltip == null) 
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
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

        ItemData item3 = new ItemData();
        item3.itemID = 3;
        item3.itemOriginID = 0;
        item3.itemImage = null;
        item3.itemName = "����������";
        item3.itemDescription = "������ +5";
        item3.itemFunction = gameObject.AddComponent<ItemDamegeUp>();
        item3.itemGetNumbers = 0;
        items.Add(item3);

        ItemData item4 = new ItemData();
        item4.itemID = -1;
        item4.itemOriginID = 0;
        item4.itemImage = null;
        item4.itemName = "�˼����»���";
        item4.itemDescription = "�ȿ� ������ ������ �𸥴�";
        item4.itemFunction = gameObject.AddComponent<ItemDropBox>();
        item4.itemGetNumbers = 0;
        items.Add(item4);

    }

    /*
        ItemData item = new ItemData();
        item.itemID = ;
        item.itemOriginID = 0;
        item.itemImage = null;
        item.itemName = "";
        item.itemDescription = "";
        item.itemFunction = gameObject.AddComponent<>();
        item.itemGetNumbers = 0;
        items.Add(item);
     */

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
            if (itemID > 0) 
            {
                ItemsList.Add(ItemDatabase.instance.GetItemByID(itemID).itemImage);
                itemindex.Add(ItemDatabase.instance.GetItemByID(itemID).itemID);
                getItemCount++;
            }


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
    public ItemTooltip itemTool()
    {
        return itemTooltip;
    }
}


