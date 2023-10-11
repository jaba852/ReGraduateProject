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
    public AudioSource audioSource; // ����� �ҽ� ������Ʈ

    public Image imageToFade;
    public float fadeDuration = 5f;

    private Color startColor;
    private Color targetColor = new Color(0f, 0f, 0f, 1f); // �ִ� ���� (���� 1)

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
                // ���� ī�޶� ����� AudioSource ������Ʈ�� ã���ϴ�.
                AudioSource audioSource = mainCamera.GetComponent<AudioSource>();

                if (audioSource != null)
                {
                    // AudioSource ������Ʈ�� �����մϴ�.
                    Destroy(audioSource);
                }
                else
                {
                    Debug.Log("���� ī�޶� AudioSource ������Ʈ�� �����ϴ�.");
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
        MSpeed.text = "�̵��ӵ�: " + stats.movementSpeed.ToString();
        SAttack.text = "���ݼӵ�: " + stats.atkSpeed.ToString();
        Power.text = "���ݷ�: " + stats.power.ToString();
        attackAdd.text = "�߰�������: " + stats.attackAddness.ToString();
        DashStack.text = "������ �ִ� ����: " + stats.dashStack.ToString();
        DashCoolDown.text = "������ ��Ÿ��: " + stats.DashCoolDown.ToString();
        QCoolDown.text = "Q��ų ��Ÿ��: " + stats.QCoolDown.ToString();
        ECoolDown.text = "E��ų ��Ÿ��: " + stats.ECoolDown.ToString();


       
    }
    private IEnumerator FadeInImage()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration); // ������ (0 ~ 1)

            // ������ ���� ���� ����
            Color lerpedColor = Color.Lerp(startColor, targetColor, t);
            imageToFade.color = lerpedColor;

            yield return null;
        }

        // ���̵尡 ���� �� ���ϴ� �۾��� ������ �� �ֽ��ϴ�.
        Debug.Log("���̵尡 �Ϸ�Ǿ����ϴ�.");
    }
}   
