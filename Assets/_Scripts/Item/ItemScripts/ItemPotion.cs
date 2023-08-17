using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPotion : MonoBehaviour, IItemFunction
{
    [SerializeField]
    public WarriorStatus status;
    private ItemData item;

    public void ItemUsed(GameObject itemObject)
    {
        
        item.itemGetNumbers += 1;
        status.TakeDamagePlayer(-10);
        Destroy(itemObject);
    }
    public void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        status = playerObject.GetComponent<WarriorStatus>();
        item = ItemDatabase.instance.GetItemByID(1);
    }

}
