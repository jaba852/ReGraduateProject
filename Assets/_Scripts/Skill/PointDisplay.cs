using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointDisplay : MonoBehaviour
{
    public PointSystem pointSystem; // Inspector에서 PointSystem을 할당하세요
    private TextMeshProUGUI pointText; // 또는 'private Text pointText;'를 사용하세요

    void Start()
    {
        pointText = GetComponent<TextMeshProUGUI>(); // 또는 'pointText = GetComponent<Text>();'를 사용하세요
    }

    void Update()
    {
        pointText.text = "" + pointSystem.CurrentPoints;
    }
}
