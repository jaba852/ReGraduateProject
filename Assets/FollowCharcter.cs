using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharcter : MonoBehaviour
{
    public Transform target; // ���� ���(�÷��̾�)�� �����ϱ� ���� public ����
    public Vector3 offset = new Vector3(0f, 2f, -10f); // ī�޶�� ��� ���� �Ÿ� �� ��ġ ����

    private void Update()
    {
        if (target != null)
        {
            // ī�޶��� ��ġ�� ����� ��ġ�� offset�� ���� ��ġ�� ����
            transform.position = target.position + offset;
        }
    }
}
