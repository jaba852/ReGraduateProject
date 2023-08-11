using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpBar : MonoBehaviour
{
    public Slider expbar;  // 체력바로 작동하는 이미지를 참조합니다.
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

        // 체력 정보가 변경될 때마다 UI 업데이트하는 이벤트 핸들러 등록
        stats.ExpChanged += UpdateExpUI;

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
        expText.text = "Exp : " + stats.playerExp.ToString() + "/" + stats.maxExp.ToString();
        LevelText.text = stats.playerLevel.ToString();
    }
}
