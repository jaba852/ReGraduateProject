using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;  // 체력바로 작동하는 이미지를 참조합니다.

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }    
}