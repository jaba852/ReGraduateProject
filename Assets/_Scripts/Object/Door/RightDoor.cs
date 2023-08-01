using UnityEngine;

public class RightDoor : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject mainCamera; // 메인 카메라 참조 변수 추가
    private bool canMove = false;
    [SerializeField]
    private float moveDistance = 12f;

    [SerializeField] // 적이 보일 때 변경할 스프라이트를 참조하기 위한 변수
    private Sprite alternateSprite;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject; // 메인 카메라 참조

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
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
            playerPosition.x += moveDistance;

            // 2D 맵에서 플레이어 오브젝트를 이동시킴
            playerObject.transform.position = playerPosition;
        }

        // 적이 보이는지 확인하고 스프라이트를 변경
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
