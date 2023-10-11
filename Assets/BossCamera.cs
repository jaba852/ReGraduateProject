using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ�� Transform ������Ʈ�� �Ҵ��� ����
    private Vector3 offset; // ī�޶�� �÷��̾� ���� ��� ��ġ

    private bool isCameraFixed = false; // ī�޶� �������� ���θ� �����ϴ� ����

    public float moveSpeed = 5.0f; // ī�޶� �̵� �ӵ�

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
            // ������ ��ġ�� �̵�
            Vector3 fixedPosition = new Vector3(0, 0, -10);
            transform.position = Vector3.Lerp(transform.position, fixedPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void FixCamera()
    {
        isCameraFixed = true;
    }
}