using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectTestRoom : MonoBehaviour
{
    public GameObject tvObject; // TV 오브젝트
    public Transform leftPlatform; // 왼쪽 발판의 Transform
    public Transform rightPlatform; // 오른쪽 발판의 Transform
    public string mapGeneratorSceneName = "MapGenerator"; // MapGenerator 씬의 이름

    private Camera tvCamera; // TV 화면을 렌더링할 카메라
    private RenderTexture renderTexture; // TV 화면에 렌더링된 씬을 저장할 텍스처
    private bool isMapGeneratorActive = false; // 맵 생성기 씬이 실행 중인지 여부를 나타내는 플래그

    private void Start()
    {
        tvObject = GameObject.Find("TVObject");

        // TV 오브젝트에서 카메라 컴포넌트 가져오기
        tvCamera = tvObject.GetComponentInChildren<Camera>();

        // Render Texture 생성
        renderTexture = new RenderTexture(256, 256, 0); // 원하는 크기로 설정

        // TV 카메라의 Target Texture 설정
        tvCamera.targetTexture = renderTexture;
    }

    private void Update()
    {
        // 왼쪽 발판과의 충돌을 감지하여 맵 생성기 씬을 실행하거나 재시작
        if (IsCollidingWithLeftPlatform() && !isMapGeneratorActive)
        {
            StartMapGeneratorScene();
        }
        // 오른쪽 발판과의 충돌을 감지하여 맵 생성기 씬을 종료
        else if (IsCollidingWithRightPlatform() && isMapGeneratorActive)
        {
            StopMapGeneratorScene();
        }
    }

    private bool IsCollidingWithLeftPlatform()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(leftPlatform.position, leftPlatform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player")) // 캐릭터와의 충돌을 감지
                return true;
        }
        return false;
    }

    private bool IsCollidingWithRightPlatform()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(rightPlatform.position, rightPlatform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player")) // 캐릭터와의 충돌을 감지
                return true;
        }
        return false;
    }

    private void StartMapGeneratorScene()
    {
        SceneManager.LoadScene(mapGeneratorSceneName, LoadSceneMode.Additive);
        isMapGeneratorActive = true;
    }

    private void StopMapGeneratorScene()
    {
        SceneManager.UnloadSceneAsync(mapGeneratorSceneName);
        isMapGeneratorActive = false;
    }
}
