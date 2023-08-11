using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;  // ü�¹ٷ� �۵��ϴ� �̹����� �����մϴ�.
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

        // ü�� ������ ����� ������ UI ������Ʈ�ϴ� �̺�Ʈ �ڵ鷯 ���
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
