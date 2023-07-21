using UnityEngine;

public class LeftDoor : MonoBehaviour
{
    private GameObject playerObject;
    private bool canMove = false;
    [SerializeField]
    private float moveDistance = 12f;
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
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
            playerPosition.x -= moveDistance;

            // 2D �ʿ��� �÷��̾� ������Ʈ�� �̵���Ŵ
            playerObject.transform.position = playerPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ����� �˸� ���
            Debug.Log("�÷��̾�� �浹�߽��ϴ�.");
            canMove = true;
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
}