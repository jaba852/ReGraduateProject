using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMace : MonoBehaviour
{
    public double MaceDamage = 5;
    public float Macespeed = 100;
    public int Macecount = 3;
    public float AttackDelay = 1f;
    public GameObject prefab; // ������ ������Ʈ�� ������
    private bool isAttack = true;
    private GameObject PlayerObject;
    public void MaceAttack() 
    {
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
        public void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        if (transform.parent != null)
        {

            // ĳ���� ������ ���� ȸ�� �� ����
            Vector3 rotationAxis = PlayerObject.transform.forward;

            // ȸ�� �ӵ��� ���� ������Ʈ�� ȸ����Ŵ
            transform.RotateAround(PlayerObject.transform.position, rotationAxis, Macespeed * Time.deltaTime);
        }
    }

}
