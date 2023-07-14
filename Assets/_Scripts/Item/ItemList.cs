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
                controller.ItemName = "�ӱ׽����� ����";
                controller.ItemValue = "������ ������ ��������.";
                break;
            case "ItemMace":
                controller.ItemName = "�ݼӸ�����";
                controller.ItemValue = "��ģ��鿡�� �̰� ������";
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
