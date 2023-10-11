using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpBar : MonoBehaviour
{
    public Slider expbar;  // ü�¹ٷ� �۵��ϴ� �̹����� �����մϴ�.
    public TextMeshProUGUI expText;
    public TextMeshProUGUI LevelText;
    public WarriorStatus stats;

    private void Start()
    {
        if (stats == null)
        {
            Debug.LogError("WarriorStatus is not assigned!");
            return;
        }
        InitializeExpBar();

        // ü�� ������ ����� ������ UI ������Ʈ�ϴ� �̺�Ʈ �ڵ鷯 ���
        stats.ExpChanged += UpdateExpUI;

    }
    private void Update()
    {
        UpdateExpUI();


    }
    private void OnDestroy()
    {
        stats.ExpChanged -= UpdateExpUI;
    }
    public void InitializeExpBar()
    {
        expbar.maxValue = stats.maxExp;
        UpdateExpUI();
    }

    public void UpdateExpUI()
    {
        expbar.value = stats.playerExp;

        float expPercentage = (float)stats.playerExp / stats.maxExp * 100f;
        expText.text = expPercentage.ToString("0") + "%";

        LevelText.text = stats.playerLevel.ToString();
    }

}
