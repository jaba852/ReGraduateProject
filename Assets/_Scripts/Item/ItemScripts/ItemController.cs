using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ItemController : MonoBehaviour
{
 
    [SerializeField]
    private int ItemID = 0;
    [SerializeField]
    public ItemInformation ItemInformation;

    public float shakeAmount = 0.1f; // 흔들림 정도
    public float shakeSpeed = 5f; // 흔들림 속도
    
    private string objectName;

    private Vector2 initialPosition;
    private float time;
    
    public string ItemName = "Test Name";
    public string ItemDescription = "Test Information";

    private bool isUsing = true;
    private ItemData item;
    private SpriteRenderer itemImage;
   
    public void Awake()
    {
        ItemInformation = FindObjectOfType<ItemInformation>();
        if (ItemInformation == null)
        {
            ItemInformation = ItemDatabase.instance.itemInfor();
        }
 
        if (ItemID == 0) { Debug.Log("ID값이 안들어감"); }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            item = ItemDatabase.instance.GetItemByID(ItemID);
            if (item != null)
            {
                item.itemImage = itemImage.sprite;
                ItemInformation.gameObject.SetActive(true);
                ItemName = item.itemName;
                ItemDescription = item.itemDescription;
                ItemInformation.SetupTooltip(ItemName, ItemDescription, initialPosition);


            }
            else
            {
                Debug.Log("아이템을 찾지못함");
            }

        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F) && isUsing && Inventory.Instance.AddItem())
            {
                // 특정 키를 누르면 아이템 활성화
              
                UseItem();
                isUsing = false;
            }


        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            ItemInformation.gameObject.SetActive(false);
        }
    }

    private void UseItem()
    {
        Debug.Log("아이템을 사용합니다.");
        ItemInformation.gameObject.SetActive(false);
        item.itemGetNumbers += 1;
        ItemDatabase.instance.UseItemByID(ItemID, gameObject);
        Inventory.Instance.AddItem();
        // 회복된 후 UI 요소 비활성화



    }


    private void Start()
    {
        initialPosition = transform.position;
        objectName = gameObject.name;
        itemImage = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 흔들림 효과 계산
        float offset = Mathf.Sin(time * shakeSpeed) * shakeAmount;
        Vector2 shakePosition = initialPosition + new Vector2(0f, offset);

        // 오브젝트 위치 업데이트
        transform.position = shakePosition;

        // 시간 증가
        time += Time.deltaTime;

    }


}
