using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public WarriorStatus stats;
    public Text MSpeed;
    public Text SAttack;
    public Text Power;
    public Text attackAdd;
    public Text DashStack;
    public Text DashCoolDown;
    public Text QCoolDown;
    public Text ECoolDown;
    public GameObject canvas;
    public GameObject SkillCanvas;
    public GameObject GameOverCanvas;

    public AudioClip GmOver;
    public AudioSource breakaudio;
    public AudioSource audioSource; // 오디오 소스 컴포넌트

    public Image imageToFade;
    public float fadeDuration = 5f;

    private Color startColor;
    private Color targetColor = new Color(0f, 0f, 0f, 1f); // 최대 투명도 (투명도 1)

    public bool asd = true;
    private void Start()
    {
        asd = true;
        startColor = imageToFade.color;
        audioSource = GetComponent<AudioSource>();

        canvas.SetActive(false);
        SkillCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
    }
    void Update()
    {
        if (stats.deadCount)
        {
            canvas.SetActive(false);
            SkillCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                // 메인 카메라에 연결된 AudioSource 컴포넌트를 찾습니다.
                AudioSource audioSource = mainCamera.GetComponent<AudioSource>();

                if (audioSource != null)
                {
                    // AudioSource 컴포넌트를 제거합니다.
                    Destroy(audioSource);
                }
                else
                {
                    Debug.Log("메인 카메라에 AudioSource 컴포넌트가 없습니다.");
                }
            }
            if (asd)
            {
                audioSource.PlayOneShot(GmOver);
                asd = false;

            }

            StartCoroutine(FadeInImage());

        }
        if (Input.GetKeyDown(KeyCode.Tab) && stats.deadCount== false)
        {
            canvas.SetActive(!canvas.activeSelf);
            SkillCanvas.SetActive(!SkillCanvas.activeSelf);
        }
        MSpeed.text = "이동속도: " + stats.movementSpeed.ToString();
        SAttack.text = "공격속도: " + stats.atkSpeed.ToString();
        Power.text = "공격력: " + stats.power.ToString();
        attackAdd.text = "추가데미지: " + stats.attackAddness.ToString();
        DashStack.text = "구르기 최대 스택: " + stats.dashStack.ToString();
        DashCoolDown.text = "구르기 쿨타임: " + stats.DashCoolDown.ToString();
        QCoolDown.text = "Q스킬 쿨타임: " + stats.QCoolDown.ToString();
        ECoolDown.text = "E스킬 쿨타임: " + stats.ECoolDown.ToString();


       
    }
    private IEnumerator FadeInImage()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration); // 보간값 (0 ~ 1)

            // 보간된 투명도 값을 설정
            Color lerpedColor = Color.Lerp(startColor, targetColor, t);
            imageToFade.color = lerpedColor;

            yield return null;
        }

        // 페이드가 끝난 후 원하는 작업을 수행할 수 있습니다.
        Debug.Log("페이드가 완료되었습니다.");
    }
}   
