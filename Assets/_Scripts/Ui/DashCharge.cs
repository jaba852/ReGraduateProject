using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCharge : MonoBehaviour
{
    public WarriorMovement warriorMovement;
    public Text dashChargeText;

    private void Update()
    {
        dashChargeText.text = warriorMovement.currentDashStack.ToString();
    }
}

