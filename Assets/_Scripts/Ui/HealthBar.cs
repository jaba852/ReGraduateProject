using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;  // 체력바로 작동하는 이미지를 참조합니다.
    public TextMeshProUGUI HPText;
    public WarriorStatus stats;

    private void Start()
    {
        if (stats == null)
        {
            Debug.LogError("WarriorStatus is not assigned!");
            return;
        }
        InitializeHealthBar();

        // 체력 정보가 변경될 때마다 UI 업데이트하는 이벤트 핸들러 등록
        stats.HealthChanged += UpdateHealthUI;

    }
    private void OnDestroy()
    {
        stats.HealthChanged -= UpdateHealthUI;
    }
    public void InitializeHealthBar()
    {
        healthBar.maxValue = stats.maxHealth;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthBar.value = stats.currentHealth;
        HPText.text = "HP : " + stats.currentHealth.ToString() + "/" + stats.maxHealth.ToString();
    }
}
