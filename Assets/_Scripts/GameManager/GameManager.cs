using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false; // 게임 일시정지 여부를 저장할 변수
    public GameObject pauseCanvas; // 일시정지시 나타날 캔버스
    public GameObject settingsCanvas; // 환경설정 버튼

    private void Awake()
    {
        // 게임매니저 오브젝트를 씬 전환시에도 유지
        DontDestroyOnLoad(gameObject);

        // 캔버스 비활성화
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);


    }

    private void Update()
    {
        // ESC 키를 눌렀을 때 일시정지/해제 기능 구현
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
        // 게임 일시정지 처리
        isPaused = true;
        Time.timeScale = 0; // 게임 시간을 0으로 설정하여 정지

        // 캔버스 활성화
        pauseCanvas.SetActive(true);

    }


    public void ResumeGame()
    {
        Debug.Log("재시작버튼");
        // 게임 일시정지 해제 처리
        isPaused = false;
        Time.timeScale = 1; // 게임 시간을 1로 설정하여 재개

        // 캔버스 비활성화
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

    }


    public void GoToScene()
    {
        Debug.Log("재시작버튼");
        isPaused = false;

        Time.timeScale = 1; // 게임 시간을 1로 설정하여 재개
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        // 지정된 씬으로 이동
        SceneManager.LoadScene(1);


    }
    public void OpenSettings()
    {
        // 환경설정 UI를 활성화
        settingsCanvas.SetActive(true);
    }
    public void QuitGame()
    {
        // 게임 종료
        Application.Quit();
    }
}
