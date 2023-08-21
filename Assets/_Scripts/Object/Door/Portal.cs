using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject mainCamera;
    private Animator animator;
    private float moveDistance = 15.0f;
    private float cooldownTime = 1.0f; // 재이동 대기 시간
    private bool canMove = true; // 초기에는 이동 가능
    private Collider2D portalCollider; // 트리거 콜라이더

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject;

        animator = GetComponent<Animator>();
        portalCollider = GetComponent<Collider2D>();

        // 애니메이션을 시작합니다.
        StartCoroutine(AnimateLoop());
    }

    private IEnumerator AnimateLoop()
    {
        float waitTime = 0.5f; // 대기 시간 (예: 0.5초)

        while (true)
        {
            bool areEnemiesVisible = IsEnemyVisible();
            //Debug.Log(areEnemiesVisible);
            // Portal 상태 실행
            animator.SetBool("PortalTrigger", areEnemiesVisible);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !IsEnemyVisible() && canMove)
        {
            MovePlayer();
            StartCoroutine(DisablePortalForCooldown());
        }
    }

    private void MovePlayer()
    {
        Vector3 playerPosition = playerObject.transform.position;

        if (gameObject.CompareTag("PortalTop"))
        {
            playerPosition.y += moveDistance;
        }
        else if (gameObject.CompareTag("PortalBottom"))
        {
            playerPosition.y -= moveDistance;
        }
        else if (gameObject.CompareTag("PortalLeft"))
        {
            playerPosition.x -= moveDistance;
        }
        else if (gameObject.CompareTag("PortalRight"))
        {
            playerPosition.x += moveDistance;
        }

        playerObject.transform.position = playerPosition;
    }

    private IEnumerator DisablePortalForCooldown()
    {
        canMove = false;
        portalCollider.enabled = false;

        yield return new WaitForSeconds(cooldownTime); // 이 시간 동안 재이동 불가능

        canMove = true;
        portalCollider.enabled = true;
    }

    // 메인 카메라에서 Enemy 태그를 가진 오브젝트를 찾아 시야에 보이는지 검사하는 함수
    private bool IsEnemyVisible()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPoint = mainCamera.GetComponent<Camera>().WorldToViewportPoint(enemy.transform.position);
            bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (onScreen)
            {
                return true;
            }
        }
        return false;
    }
}
