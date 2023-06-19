using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;  // 체력바로 작동하는 이미지를 참조합니다.
    public WarriorStatus warriorStatus;  // 체력 정보를 가져오기 위해 워리어 상태를 참조합니다.

    void Start()
    {
        healthBar.fillAmount = (float)warriorStatus.currentHealth / warriorStatus.maxHealth;
    }

    void Update()
    {
        if (warriorStatus.currentHealth <= 0)
        {
            // 워리어의 체력이 0 이하라면, 체력바를 0으로 설정합니다.
            healthBar.fillAmount = 0;
        }
        else
        {
            healthBar.fillAmount = (float)warriorStatus.currentHealth / warriorStatus.maxHealth;
        }
    }
}