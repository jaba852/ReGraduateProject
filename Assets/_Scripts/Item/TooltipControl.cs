using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class TooltipControl : MonoBehaviour,IPointerExitHandler,IPointerMoveHandler
{
    public ItemTooltip itemTooltip; 
    public ItemDatabase itemDatabase;
    public Vector2 initialPosition;
    public void Awake()
    {
        itemTooltip = FindObjectOfType<ItemTooltip>();
        if (itemTooltip == null)
        {
            itemTooltip = ItemDatabase.instance.itemTool();
        }
        itemDatabase = FindObjectOfType<ItemDatabase>();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltip.RemoveTooltip();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Slot item = GetComponent<Slot>();
        if (item != null)
        {
            int slotIndex = 0;
            slotIndex = item.SlotIndex;
            if (slotIndex != 0)
            {
                Debug.Log(itemDatabase.GetItemByID(slotIndex).itemName);
                Debug.Log(itemDatabase.GetItemByID(slotIndex).itemDescription);
                Debug.Log(transform.position);
                itemTooltip.SetupTooltip(itemDatabase.GetItemByID(slotIndex).itemName, itemDatabase.GetItemByID(slotIndex).itemDescription, initialPosition);

            }

        }
    }



    private void Update()
    {
        initialPosition = Input.mousePosition;

    }

}
