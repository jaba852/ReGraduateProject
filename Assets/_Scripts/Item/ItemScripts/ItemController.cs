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

    public float shakeAmount = 0.1f; // ��鸲 ����
    public float shakeSpeed = 5f; // ��鸲 �ӵ�
    
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
 
        if (ItemID == 0) { Debug.Log("ID���� �ȵ�"); }
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
                Debug.Log("�������� ã������");
            }

        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F) && isUsing && Inventory.Instance.AddItem())
            {
                // Ư�� Ű�� ������ ������ Ȱ��ȭ
              
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
        Debug.Log("�������� ����մϴ�.");
        ItemInformation.gameObject.SetActive(false);
        item.itemGetNumbers += 1;
        ItemDatabase.instance.UseItemByID(ItemID, gameObject);
        Inventory.Instance.AddItem();
        // ȸ���� �� UI ��� ��Ȱ��ȭ



    }


    private void Start()
    {
        initialPosition = transform.position;
        objectName = gameObject.name;
        itemImage = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // ��鸲 ȿ�� ���
        float offset = Mathf.Sin(time * shakeSpeed) * shakeAmount;
        Vector2 shakePosition = initialPosition + new Vector2(0f, offset);

        // ������Ʈ ��ġ ������Ʈ
        transform.position = shakePosition;

        // �ð� ����
        time += Time.deltaTime;

    }


}
