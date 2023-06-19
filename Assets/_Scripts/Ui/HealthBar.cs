using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;  // ü�¹ٷ� �۵��ϴ� �̹����� �����մϴ�.
    public WarriorStatus warriorStatus;  // ü�� ������ �������� ���� ������ ���¸� �����մϴ�.

    void Start()
    {
        healthBar.fillAmount = (float)warriorStatus.currentHealth / warriorStatus.maxHealth;
    }

    void Update()
    {
        if (warriorStatus.currentHealth <= 0)
        {
            // �������� ü���� 0 ���϶��, ü�¹ٸ� 0���� �����մϴ�.
            healthBar.fillAmount = 0;
        }
        else
        {
            healthBar.fillAmount = (float)warriorStatus.currentHealth / warriorStatus.maxHealth;
        }
    }
}