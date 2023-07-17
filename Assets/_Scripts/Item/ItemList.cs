using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField]
    public ItemController controller;
    [SerializeField]
    public WarriorStatus status;
    [SerializeField]
    public ItemMace ItemMace;

    public void Awake()
    {
        if(status == null)
        {
            status = FindObjectOfType<WarriorStatus>();
        }
        
        // status = new WarriorStatus();
    }
    public void ItemSearch(string objectName) 
    {
        switch (objectName) 
        {
            case "ItemPotion":
                controller.ItemName = "붉그스름한 포션";
                controller.ItemValue = "끈적한 점성이 느껴진다.";
                break;
            case "ItemMace":
                controller.ItemName = "금속몽둥이";
                controller.ItemValue = "미친놈들에겐 이게 약이지";
                break;
            default:
                break;
        }

    }

    public void ItemUsed(string objectName) 
    {
        switch (objectName)
        {
            case "ItemPotion":
                status.TakeDamagePlayer(-10);
                Destroy(gameObject);
                break;
            case "ItemMace":
                if (ItemMace != null)
                {
                    controller.enabled = false;
                    ItemMace.MaceAttack();
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }

    }
}
