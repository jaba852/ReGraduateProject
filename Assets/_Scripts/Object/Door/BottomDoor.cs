using UnityEngine;

public class BottomDoor : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject mainCamera; // 메인 카메라 참조 변수 추가
    private bool canMove = false;
    [SerializeField]
    private float moveDistance = 12f;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject; // 메인 카메라 참조
    }

    private void Update()
    {
        if (canMove && Input.GetKeyDown(KeyCode.F))
        {
            // 디버그 알림 출력
            Debug.Log("F 키를 눌렀습니다.");

            // 플레이어 태그를 가진 오브젝트의 좌표 값을 가져옴
            Vector3 playerPosition = playerObject.transform.position;

            // y 좌표를 -12f 만큼 이동
            playerPosition.y -= moveDistance;

            // 2D 맵에서 플레이어 오브젝트를 이동시킴
            playerObject.transform.position = playerPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 메인 카메라의 시야 안에 적 캐릭터가 없을 때만 상호작용 가능하도록
            if (!IsEnemyVisible())
            {
                // 디버그 알림 출력
                Debug.Log("플레이어와 충돌했습니다.");
                canMove = true;
            }
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
