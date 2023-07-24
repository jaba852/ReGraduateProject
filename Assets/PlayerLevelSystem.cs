using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLevelSystem : MonoBehaviour
{
    public int maxLevel = 20; // 최대 레벨
    public int currentLevel = 1; // 현재 레벨
    public int currentExp = 0; // 현재 경험치
    public int[] expToNextLevel;
    public int skillPointsPerLevel = 1; // 레벨업 시 얻는 스킬 포인트
    public Text levelText; // 레벨을 표시하는 UI 텍스트
    public Text expText; // 경험치를 표시하는 UI 텍스트
    public GameObject[] skillPanels; // 스킬창 UI 패널
    private GameObject currentSkillPanel; // 현재 활성화된 스킬 패널

    // 싱글톤 인스턴스
    private static PlayerLevelSystem instance;

    // 외부에서 접근 가능한 인스턴스 프로퍼티
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
                    DontDestroyOnLoad(singletonObject); // 씬 전환 시에도 파괴되지 않도록 설정
                }
            }
            return instance;
        }
    }


    // 초기화
    private void Start()
    {
        skillPanels = new GameObject[maxLevel];
        for (int currentLevel = 1; currentLevel <= maxLevel; currentLevel++)
        {
            skillPanels[currentLevel - 1] = GameObject.Find("SkillPanel" + currentLevel);
            if (skillPanels[currentLevel - 1] != null)
            {
                skillPanels[currentLevel - 1].SetActive(false); // 시작 시 스킬 패널들을 모두 비활성화합니다.
            }
        }
        // 레벨별 필요 경험치 배열 초기화
        expToNextLevel = new int[maxLevel];
        int exp = 10;
        for (int level = 1; level <= maxLevel; level++)
        {
            expToNextLevel[level - 1] = exp;
            exp += 10;
        }
        UpdateUI();
    }

    // 경험치 추가
    public void AddExperience(int amount)
    {
       
        currentExp += amount;
        Debug.Log("Received " + amount + " EXP. Current EXP: " + currentExp);
        if (currentLevel >= maxLevel)
        {
            // 이미 최대 레벨에 도달한 경우
            currentExp = 0;
            return;
        }

        // 레벨업 체크
        while (currentLevel < maxLevel && currentExp >= expToNextLevel[currentLevel - 1])
        {
            currentExp -= expToNextLevel[currentLevel - 1];
            LevelUp();
        }

        UpdateUI();
    }

    // 레벨업
    private void LevelUp()
    {
        if (currentLevel >= maxLevel)
            return;
        currentExp -= expToNextLevel[currentLevel -1];
        currentLevel++;
        skillPointsPerLevel++; // 레벨업 시 스킬 포인트 증가

        Debug.Log("Level Up! Current Level: " + currentLevel + ", Current EXP: " + currentExp);

        // 스킬창 활성화
        currentSkillPanel = skillPanels[currentLevel - 1];
        if (currentSkillPanel != null)
        {
            currentSkillPanel.SetActive(true);
        }

        // TODO: 스킬창 UI 업데이트 처리
    }

    // 스킬 포인트 할당
    public void AssignSkillPoints(int skillPoints)
    {
        // TODO: 스킬 포인트에 따른 스킬 업그레이드 또는 새로운 스킬 배우기 등의 처리

        currentSkillPanel.SetActive(false); // 스킬창 비활성화
    }

    // UI 업데이트
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