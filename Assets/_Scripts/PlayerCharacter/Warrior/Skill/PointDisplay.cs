using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointDisplay : MonoBehaviour
{
    public PointSystem pointSystem; // Inspector���� PointSystem�� �Ҵ��ϼ���
    private TextMeshProUGUI pointText; // �Ǵ� 'private Text pointText;'�� ����ϼ���

    void Start()
    {
        pointText = GetComponent<TextMeshProUGUI>(); // �Ǵ� 'pointText = GetComponent<Text>();'�� ����ϼ���
    }

    void Update()
    {
        pointText.text = "" + pointSystem.CurrentPoints;
    }
}
