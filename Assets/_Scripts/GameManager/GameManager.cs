using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false; // ���� �Ͻ����� ���θ� ������ ����
    public GameObject pauseCanvas; // �Ͻ������� ��Ÿ�� ĵ����
    public GameObject settingsCanvas; // ȯ�漳�� ��ư
    public GameObject talkCanvas;
    public TextMeshProUGUI talkText;
    public bool isTalkAction = false;
    public GameObject gmBreaker;
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
        
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

    }


    public void GoToScene()
    {
        isPaused = false;

        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        SceneManager.LoadScene(1);

        DestroyObject(gmBreaker);
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

    public void TalkAction(GameObject scanObj) 
    {
        if (isTalkAction)
        {
            isTalkAction = false;
            talkCanvas.SetActive(false);
        }
        else
        {
            isTalkAction = true;
            talkCanvas.SetActive(true);
            talkText.text = scanObj.name;
        }
    }
}
