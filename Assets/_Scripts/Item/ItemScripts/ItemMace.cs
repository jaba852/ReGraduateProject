using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemMace : MonoBehaviour, IItemFunction
{
    public double MaceDamage = 5;
    public float Macespeed = 100;
    public int Macecount = 3;
    public float AttackDelay = 1f;
    public GameObject prefab; // 생성할 오브젝트의 프리팹
    private bool isAttack = true;
    private GameObject PlayerObject;
    private ItemData item;


    public void Start()
    {

        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        prefab = Resources.Load<GameObject>("Prefabs/ItemSpinMace");
        item = ItemDatabase.instance.GetItemByID(2);

    }

    public void ItemUsed(GameObject itemObject) 
    {
       
        ItemController itemController = itemObject.GetComponent<ItemController>();
        
        if (itemController != null)
        {
            itemController.enabled = false; // ItemController 비활성화
        }
        item.itemGetNumbers += 1;
        Debug.Log("메이스 획득");
        
        if (PlayerObject != null)
        {
            for (int index = 0; index < Macecount; index++) 
            {
                // 오브젝트를 생성하고 부모로 설정합니다.
                GameObject obj = Instantiate(prefab, PlayerObject.transform.position, Quaternion.identity);
                obj.transform.SetParent(PlayerObject.transform);

                // 회전 및 이동을 수행합니다.
                Vector3 rotVec = Vector3.forward * 360f * index / Macecount;
                obj.transform.Rotate(rotVec);
                obj.transform.Translate(obj.transform.up * 2.0f, Space.World);

            }

        }
        Destroy(itemObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("DeepOne")&&isAttack)
        {
            isAttack = false;
            EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();
            if (enemyStatus != null)
            {
                // EnemyStatus 컴포넌트의 함수 호출
                enemyStatus.TakeDamage(MaceDamage);
                StartCoroutine(AttackDelayCoroutine());
            }
        }
    }
    private System.Collections.IEnumerator AttackDelayCoroutine()
    {
        yield return new WaitForSeconds(AttackDelay);
        isAttack = true;
    }

    public void Update()
    {
        if (transform.parent != null)
        {

            // 캐릭터 주위를 도는 회전 축 설정
            Vector3 rotationAxis = PlayerObject.transform.forward;

            // 회전 속도에 따라 오브젝트를 회전시킴
            transform.RotateAround(PlayerObject.transform.position, rotationAxis, Macespeed * Time.deltaTime);
        }


    }

}
