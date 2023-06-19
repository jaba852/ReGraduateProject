using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyPointa : MonoBehaviour
{
    private CircleCollider2D circleCollider; // ��Ŭ �ݶ��̴� 2D ����


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeepOnebomb()
    {
        // �ݸ��� ����

        GameObject collisionObject = new GameObject("CollisionDeepOnebombObject");
        Vector3 spawnPosition = transform.position + new Vector3(0f, -1.5f);
        collisionObject.transform.position = spawnPosition;
        CircleCollider2D circleCollider = collisionObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 1.1f;
        Rigidbody2D rigidbody = collisionObject.AddComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        collisionObject.tag = "Player"; // Player �±׸� �����Ͽ� �浹 ó���� �����մϴ�.

        // ���� ��� �Ŀ� �ݸ��� ����
        StartCoroutine(DestroyCollisionAfterDelay(collisionObject, 3f));
    }

    private IEnumerator DestroyCollisionAfterDelay(GameObject collisionObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(collisionObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!");
        }
    }

}

