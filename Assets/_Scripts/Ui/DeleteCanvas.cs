using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCanvas : MonoBehaviour
{
    public Canvas uiCanvas; // 숨길 캔버스
    public WarriorStatus warriorStatus; // 워리어 상태

    private void Update()
    {
        if (warriorStatus.deadCount) // 워리어가 죽었는지 확인
        {
            uiCanvas.gameObject.SetActive(false); // 캔버스를 숨김
        }
        else
        {
            uiCanvas.gameObject.SetActive(true); // 워리어가 살아 있으면 캔버스를 표시
        }
    }
}
