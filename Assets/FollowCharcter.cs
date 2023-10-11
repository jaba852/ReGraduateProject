using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharcter : MonoBehaviour
{
    public Transform target; // 따라갈 대상(플레이어)을 설정하기 위한 public 변수
    public Vector3 offset = new Vector3(0f, 2f, -10f); // 카메라와 대상 간의 거리 및 위치 조정

    private void Update()
    {
        if (target != null)
        {
            // 카메라의 위치를 대상의 위치에 offset을 더한 위치로 설정
            transform.position = target.position + offset;
        }
    }
}
