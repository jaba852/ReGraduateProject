using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyPointa : MonoBehaviour
{
    private CircleCollider2D circleCollider; // 서클 콜라이더 2D 변수


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
        // 콜리전 생성

        GameObject collisionObject = new GameObject("CollisionDeepOnebombObject");
        Vector3 spawnPosition = transform.position + new Vector3(0f, -1.5f);
        collisionObject.transform.position = spawnPosition;
        CircleCollider2D circleCollider = collisionObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 1.1f;
        Rigidbody2D rigidbody = collisionObject.AddComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        collisionObject.tag = "Player"; // Player 태그를 설정하여 충돌 처리를 위임합니다.

        // 아주 잠깐 후에 콜리전 삭제
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

