using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject mainCamera;
    private Animator animator;
    private float moveDistance = 15.0f;
    private float cooldownTime = 1.0f; // ���̵� ��� �ð�
    private bool canMove = true; // �ʱ⿡�� �̵� ����
    private Collider2D portalCollider; // Ʈ���� �ݶ��̴�

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject;

        animator = GetComponent<Animator>();
        portalCollider = GetComponent<Collider2D>();

        // �ִϸ��̼��� �����մϴ�.
        StartCoroutine(AnimateLoop());
    }

    private IEnumerator AnimateLoop()
    {
        float waitTime = 0.5f; // ��� �ð� (��: 0.5��)

        while (true)
        {
            bool areEnemiesVisible = IsEnemyVisible();
            //Debug.Log(areEnemiesVisible);
            // Portal ���� ����
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

        yield return new WaitForSeconds(cooldownTime); // �� �ð� ���� ���̵� �Ұ���

        canMove = true;
        portalCollider.enabled = true;
    }

    // ���� ī�޶󿡼� Enemy �±׸� ���� ������Ʈ�� ã�� �þ߿� ���̴��� �˻��ϴ� �Լ�
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
