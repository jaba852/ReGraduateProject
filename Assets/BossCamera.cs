using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트의 Transform 컴포넌트를 할당할 변수
    private Vector3 offset; // 카메라와 플레이어 간의 상대 위치

    private bool isCameraFixed = false; // 카메라를 고정할지 여부를 결정하는 변수

    public float moveSpeed = 5.0f; // 카메라 이동 속도

    void Start()
    {
    }

    void Update()
    {
        if (!isCameraFixed)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
        else
        {
            // 고정된 위치로 이동
            Vector3 fixedPosition = new Vector3(0, 0, -10);
            transform.position = Vector3.Lerp(transform.position, fixedPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void FixCamera()
    {
        isCameraFixed = true;
    }
}