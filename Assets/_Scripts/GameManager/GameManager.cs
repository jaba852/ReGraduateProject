using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false; // ���� �Ͻ����� ���θ� ������ ����
    public GameObject pauseCanvas; // �Ͻ������� ��Ÿ�� ĵ����
    public GameObject settingsCanvas; // ȯ�漳�� ��ư
    [SerializeField] Texture2D cursorimage;
    private void Awake()
    {
        Cursor.SetCursor(cursorimage, Vector2.zero, CursorMode.ForceSoftware);
        // ���ӸŴ��� ������Ʈ�� �� ��ȯ�ÿ��� ����
        DontDestroyOnLoad(gameObject);

        // ĵ���� ��Ȱ��ȭ
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);


    }

    private void Update()
    {   
        // ESC Ű�� ������ �� �Ͻ�����/���� ��� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    public void PauseGame()
    {
        // ���� �Ͻ����� ó��
        isPaused = true;
        Time.timeScale = 0; // ���� �ð��� 0���� �����Ͽ� ����

        // ĵ���� Ȱ��ȭ
        pauseCanvas.SetActive(true);

    }


    public void ResumeGame()
    {
        Debug.Log("����۹�ư");
        // ���� �Ͻ����� ���� ó��
        isPaused = false;
        Time.timeScale = 1; // ���� �ð��� 1�� �����Ͽ� �簳

        // ĵ���� ��Ȱ��ȭ
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

    }


    public void GoToScene()
    {
        Debug.Log("����۹�ư");
        isPaused = false;

        Time.timeScale = 1; // ���� �ð��� 1�� �����Ͽ� �簳
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        // ������ ������ �̵�
        SceneManager.LoadScene(1);


    }
    public void OpenSettings()
    {
        // ȯ�漳�� UI�� Ȱ��ȭ
        settingsCanvas.SetActive(true);
    }
    public void QuitGame()
    {
        // ���� ����
        Application.Quit();
    }
}
