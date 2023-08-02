using UnityEngine;

public class RightDoor : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject mainCamera; // ���� ī�޶� ���� ���� �߰�
    private bool canMove = false;
    [SerializeField]
    private float moveDistance = 12f;

    [SerializeField] // ���� ���� �� ������ ��������Ʈ�� �����ϱ� ���� ����
    private Sprite alternateSprite;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject; // ���� ī�޶� ����

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        if (canMove && Input.GetKeyDown(KeyCode.F))
        {
            // ����� �˸� ���
            Debug.Log("F Ű�� �������ϴ�.");

            // �÷��̾� �±׸� ���� ������Ʈ�� ��ǥ ���� ������
            Vector3 playerPosition = playerObject.transform.position;

            // x ��ǥ�� +12f ��ŭ �̵�
            playerPosition.x += moveDistance;

            // 2D �ʿ��� �÷��̾� ������Ʈ�� �̵���Ŵ
            playerObject.transform.position = playerPosition;
        }

        // ���� ���̴��� Ȯ���ϰ� ��������Ʈ�� ����
        bool areEnemiesVisible = IsEnemyVisible();
        if (areEnemiesVisible)
        {
            spriteRenderer.sprite = alternateSprite;
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ���� ī�޶��� �þ� �ȿ� �� ĳ���Ͱ� ���� ���� ��ȣ�ۿ� �����ϵ���
            if (!IsEnemyVisible())
            {
                // ����� �˸� ���
                Debug.Log("�÷��̾�� �浹�߽��ϴ�.");
                canMove = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ����� �˸� ���
            Debug.Log("�÷��̾���� �浹�� ����Ǿ����ϴ�.");
            canMove = false;
        }
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
