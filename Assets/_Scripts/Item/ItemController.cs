using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    public ItemList itemList;
    [SerializeField]
    public ItemInformation ItemInformation;
    public float shakeAmount = 0.1f; // ��鸲 ����
    public float shakeSpeed = 5f; // ��鸲 �ӵ�
    
    private string objectName;

    private Vector2 initialPosition;
    private float time;
    
    public string ItemName = "Test Name";
    public string ItemValue = "Test Information";

    private bool isUsing = true;

    public void Awake()
    {
        if (ItemInformation == null)
        {
            ItemInformation = FindObjectOfType<ItemInformation>();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            itemList.ItemSearch(objectName);
            Debug.Log(ItemInformation.name);
            ItemInformation.gameObject.SetActive(true);
            ItemInformation.SetupTooltip(ItemName, ItemValue, initialPosition);



        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F) && isUsing)
            {
                // Ư�� Ű�� ������ ü�� ȸ�� ����
                ItemInformation.gameObject.SetActive(false);
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
        itemList.ItemUsed(objectName);
        // ȸ���� �� UI ��� ��Ȱ��ȭ
  
  

    }


    private void Start()
    {
        initialPosition = transform.position;
        objectName = gameObject.name;
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
