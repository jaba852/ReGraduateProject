using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectTestRoom : MonoBehaviour
{
    public GameObject tvObject; // TV ������Ʈ
    public Transform leftPlatform; // ���� ������ Transform
    public Transform rightPlatform; // ������ ������ Transform
    public string mapGeneratorSceneName = "MapGenerator"; // MapGenerator ���� �̸�

    private Camera tvCamera; // TV ȭ���� �������� ī�޶�
    private RenderTexture renderTexture; // TV ȭ�鿡 �������� ���� ������ �ؽ�ó
    private bool isMapGeneratorActive = false; // �� ������ ���� ���� ������ ���θ� ��Ÿ���� �÷���

    private void Start()
    {
        tvObject = GameObject.Find("TVObject");

        // TV ������Ʈ���� ī�޶� ������Ʈ ��������
        tvCamera = tvObject.GetComponentInChildren<Camera>();

        // Render Texture ����
        renderTexture = new RenderTexture(256, 256, 0); // ���ϴ� ũ��� ����

        // TV ī�޶��� Target Texture ����
        tvCamera.targetTexture = renderTexture;
    }

    private void Update()
    {
        // ���� ���ǰ��� �浹�� �����Ͽ� �� ������ ���� �����ϰų� �����
        if (IsCollidingWithLeftPlatform() && !isMapGeneratorActive)
        {
            StartMapGeneratorScene();
        }
        // ������ ���ǰ��� �浹�� �����Ͽ� �� ������ ���� ����
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
            if (collider.CompareTag("Player")) // ĳ���Ϳ��� �浹�� ����
                return true;
        }
        return false;
    }

    private bool IsCollidingWithRightPlatform()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(rightPlatform.position, rightPlatform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player")) // ĳ���Ϳ��� �浹�� ����
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
