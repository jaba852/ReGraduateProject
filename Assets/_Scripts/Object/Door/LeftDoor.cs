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
            // 디버그 알림 출력
            Debug.Log("F 키를 눌렀습니다.");

            // 플레이어 태그를 가진 오브젝트의 좌표 값을 가져옴
            Vector3 playerPosition = playerObject.transform.position;

            // x 좌표를 +12f 만큼 이동
            playerPosition.x -= moveDistance;

            // 2D 맵에서 플레이어 오브젝트를 이동시킴
            playerObject.transform.position = playerPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 디버그 알림 출력
            Debug.Log("플레이어와 충돌했습니다.");
            canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 디버그 알림 출력
            Debug.Log("플레이어와의 충돌이 종료되었습니다.");
            canMove = false;
        }
    }
}