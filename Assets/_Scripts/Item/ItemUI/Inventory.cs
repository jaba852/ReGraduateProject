using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public ItemDatabase itemDatabase;

    public int ItemCount;


    private int slotCount;
    public int SlotCount
    {
        get => slotCount;
        set
        {
            slotCount = value;
            onSlotCountChange.Invoke(slotCount);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SlotCount = 5; // 인벤토리개수
        itemDatabase = FindObjectOfType<ItemDatabase>();

    }

    public bool AddItem()
    {
        if (itemDatabase.GetItemcount() < SlotCount)
        {
            ItemCount = itemDatabase.GetItemcount();
            Debug.Log(ItemCount);
            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }

            return true;
        }
        else if (itemDatabase.GetItemcount() == SlotCount)
        {
            ItemCount = itemDatabase.GetItemcount();
            Debug.Log(ItemCount);
            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }
            Debug.Log("인벤토리 가득참");
            return false;
        }


        return false;


    }

}
