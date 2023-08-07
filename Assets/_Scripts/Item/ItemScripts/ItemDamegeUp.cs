using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamegeUp : MonoBehaviour, IItemFunction
{
    // Start is called before the first frame update
    [SerializeField]
    public WarriorStatus status;
    private ItemData item;
    public int DamegeUp = 5;
    public void ItemUsed(GameObject itemObject)
    {

        item.itemGetNumbers += 1;
        status.power+= DamegeUp;// 데미지 증가
        Destroy(itemObject);
    }
    public void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        status = playerObject.GetComponent<WarriorStatus>();
        item = ItemDatabase.instance.GetItemByID(3);
    }
}
