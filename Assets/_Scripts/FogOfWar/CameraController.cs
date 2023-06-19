using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            Vector3 currentPosition = transform.position;
            Vector3 desiredPosition = new Vector3(targetPosition.x, targetPosition.y, currentPosition.z);
            transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, smoothTime);
        }
    }
}

