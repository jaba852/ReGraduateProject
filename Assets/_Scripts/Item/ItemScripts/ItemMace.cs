using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemMace : MonoBehaviour, IItemFunction
{
    public double MaceDamage = 5;
    public float Macespeed = 100;
    public int Macecount = 3;
    public float AttackDelay = 1f;
    public GameObject prefab; // ������ ������Ʈ�� ������
    private bool isAttack = true;
    private GameObject PlayerObject;
    private ItemData item;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start(); // Start �޼��� ������ ���⼭ ȣ��
  

    }


    public void Start()
    {

        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        if (PlayerObject == null)
        {
            InvokeRepeating("CheckAndFindPlayer", 1.0f, 1.0f);
        }
        prefab = Resources.Load<GameObject>("Prefabs/ItemSpinMace");
        item = ItemDatabase.instance.GetItemByID(2);

    }
    private void CheckAndFindPlayer()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");

        if (PlayerObject != null)
        {
            // playerObject�� ã���� ���� ����
            //Debug.Log("Player object found.");
            MoveMace();
            // ã������ InvokeRepeating �ߴ�
            CancelInvoke("CheckAndFindPlayer");
        }
        else
        {
            // playerObject�� ���� null�� ����� ����
            //Debug.Log("Player object not found yet.");
        }
    }
    private void MoveMace()
    {
        if (ItemDatabase.instance.UsedMace())
        {

            if (PlayerObject != null)
            {
                //Debug.Log("���̽����Ѿ");
                for (int index = 0; index < Macecount; index++)
                {

                    // ������Ʈ�� �����ϰ� �θ�� �����մϴ�.
                    GameObject obj = Instantiate(prefab, PlayerObject.transform.position, Quaternion.identity);
                    obj.transform.SetParent(PlayerObject.transform);

                    // ȸ�� �� �̵��� �����մϴ�.
                    Vector3 rotVec = Vector3.forward * 360f * index / Macecount;
                    obj.transform.Rotate(rotVec);
                    obj.transform.Translate(obj.transform.up * 2.0f, Space.World);

                }

            }
        }
    }
    public void ItemUsed(GameObject itemObject) 
    {
       
        ItemController itemController = itemObject.GetComponent<ItemController>();
        
        if (itemController != null)
        {
            itemController.enabled = false; // ItemController ��Ȱ��ȭ
        }
        item.itemGetNumbers += 1;
        Debug.Log("���̽� ȹ��");
        
        if (PlayerObject != null)
        {
            for (int index = 0; index < Macecount; index++) 
            {
                // ������Ʈ�� �����ϰ� �θ�� �����մϴ�.
                GameObject obj = Instantiate(prefab, PlayerObject.transform.position, Quaternion.identity);
                obj.transform.SetParent(PlayerObject.transform);

                // ȸ�� �� �̵��� �����մϴ�.
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
                // EnemyStatus ������Ʈ�� �Լ� ȣ��
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

            // ĳ���� ������ ���� ȸ�� �� ����
            if (PlayerObject != null)
            { 
                Vector3 rotationAxis = PlayerObject.transform.forward;

            // ȸ�� �ӵ��� ���� ������Ʈ�� ȸ����Ŵ
                 transform.RotateAround(PlayerObject.transform.position, rotationAxis, Macespeed * Time.deltaTime);
            }

        }


    }

}
