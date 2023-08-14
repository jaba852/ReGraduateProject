using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel;
    public bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;
    public ItemDatabase itemDatabase;
    private ItemTooltip itemTooltip;
    private void Start()
    {
        itemDatabase = FindObjectOfType<ItemDatabase>();
        itemTooltip = FindObjectOfType<ItemTooltip>();
        inven = Inventory.Instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        InventoryPanel.SetActive(activeInventory);

    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inven.SlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activeInventory = !activeInventory;
            InventoryPanel.SetActive(activeInventory);
            itemTooltip.RemoveTooltip();
        }
    }

    public void AddSlot()
    {
        inven.SlotCount++;
    }

    void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.ItemCount; i++)
        {
            slots[i].Image = itemDatabase.ItemsList[i];
            slots[i].SlotIndex = itemDatabase.itemindex[i];
            slots[i].UpdateSlotUI();
        }

    }
}