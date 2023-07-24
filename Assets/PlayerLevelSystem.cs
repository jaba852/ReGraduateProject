using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLevelSystem : MonoBehaviour
{
    public int maxLevel = 20; // �ִ� ����
    public int currentLevel = 1; // ���� ����
    public int currentExp = 0; // ���� ����ġ
    public int[] expToNextLevel;
    public int skillPointsPerLevel = 1; // ������ �� ��� ��ų ����Ʈ
    public Text levelText; // ������ ǥ���ϴ� UI �ؽ�Ʈ
    public Text expText; // ����ġ�� ǥ���ϴ� UI �ؽ�Ʈ
    public GameObject[] skillPanels; // ��ųâ UI �г�
    private GameObject currentSkillPanel; // ���� Ȱ��ȭ�� ��ų �г�

    // �̱��� �ν��Ͻ�
    private static PlayerLevelSystem instance;

    // �ܺο��� ���� ������ �ν��Ͻ� ������Ƽ
    public static PlayerLevelSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerLevelSystem>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerLevelSystemSingleton");
                    instance = singletonObject.AddComponent<PlayerLevelSystem>();
                    DontDestroyOnLoad(singletonObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
                }
            }
            return instance;
        }
    }


    // �ʱ�ȭ
    private void Start()
    {
        skillPanels = new GameObject[maxLevel];
        for (int currentLevel = 1; currentLevel <= maxLevel; currentLevel++)
        {
            skillPanels[currentLevel - 1] = GameObject.Find("SkillPanel" + currentLevel);
            if (skillPanels[currentLevel - 1] != null)
            {
                skillPanels[currentLevel - 1].SetActive(false); // ���� �� ��ų �гε��� ��� ��Ȱ��ȭ�մϴ�.
            }
        }
        // ������ �ʿ� ����ġ �迭 �ʱ�ȭ
        expToNextLevel = new int[maxLevel];
        int exp = 10;
        for (int level = 1; level <= maxLevel; level++)
        {
            expToNextLevel[level - 1] = exp;
            exp += 10;
        }
        UpdateUI();
    }

    // ����ġ �߰�
    public void AddExperience(int amount)
    {
       
        currentExp += amount;
        Debug.Log("Received " + amount + " EXP. Current EXP: " + currentExp);
        if (currentLevel >= maxLevel)
        {
            // �̹� �ִ� ������ ������ ���
            currentExp = 0;
            return;
        }

        // ������ üũ
        while (currentLevel < maxLevel && currentExp >= expToNextLevel[currentLevel - 1])
        {
            currentExp -= expToNextLevel[currentLevel - 1];
            LevelUp();
        }

        UpdateUI();
    }

    // ������
    private void LevelUp()
    {
        if (currentLevel >= maxLevel)
            return;
        currentExp -= expToNextLevel[currentLevel -1];
        currentLevel++;
        skillPointsPerLevel++; // ������ �� ��ų ����Ʈ ����

        Debug.Log("Level Up! Current Level: " + currentLevel + ", Current EXP: " + currentExp);

        // ��ųâ Ȱ��ȭ
        currentSkillPanel = skillPanels[currentLevel - 1];
        if (currentSkillPanel != null)
        {
            currentSkillPanel.SetActive(true);
        }

        // TODO: ��ųâ UI ������Ʈ ó��
    }

    // ��ų ����Ʈ �Ҵ�
    public void AssignSkillPoints(int skillPoints)
    {
        // TODO: ��ų ����Ʈ�� ���� ��ų ���׷��̵� �Ǵ� ���ο� ��ų ���� ���� ó��

        currentSkillPanel.SetActive(false); // ��ųâ ��Ȱ��ȭ
    }

    // UI ������Ʈ
    private void UpdateUI()
    {
        levelText.text = "Lv:" + currentLevel;

        if (currentLevel >= 1)
        {
            expText.text = "Exp:" + currentExp + " / " + expToNextLevel[currentLevel -1];
        }
        else
        {
            expText.text = "Exp: N/A";
        }
    }
}