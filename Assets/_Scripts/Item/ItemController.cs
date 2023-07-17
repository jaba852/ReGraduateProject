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
    public float shakeAmount = 0.1f; // 흔들림 정도
    public float shakeSpeed = 5f; // 흔들림 속도
    
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
                // 특정 키를 누르면 체력 회복 실행
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
        Debug.Log("아이템을 사용합니다.");
        ItemInformation.gameObject.SetActive(false);
        itemList.ItemUsed(objectName);
        // 회복된 후 UI 요소 비활성화
  
  

    }


    private void Start()
    {
        initialPosition = transform.position;
        objectName = gameObject.name;
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
