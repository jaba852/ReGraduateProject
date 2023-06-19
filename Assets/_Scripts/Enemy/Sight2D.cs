using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;

    [Header("View Config")]
    [Range(0f, 360f)]
    [SerializeField] private float horizontalViewAngle = 0f;
    [SerializeField] private float viewRadius = 1f;
    [Range(-180f, 180f)]
    [SerializeField] private float viewRotateZ = 0f;

    [SerializeField] private LayerMask viewTargetMask;
    [SerializeField] private LayerMask viewObstacleMask;

    private List<Collider2D> hitTargets = new List<Collider2D>();
    private float horizontalViewHalfAngle = 0f;

    private void Awake()
    {
        horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
    }

    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
            Vector3 originPos = transform.position;
            Gizmos.DrawWireSphere(originPos, viewRadius);

            Vector3 horizontalRightDir = AngleToDirZ(horizontalViewHalfAngle + viewRotateZ);
            Vector3 horizontalLeftDir = AngleToDirZ(-horizontalViewHalfAngle + viewRotateZ);
            Vector3 lookDir = AngleToDirZ(viewRotateZ);

            UnityEngine.Debug.DrawRay(originPos, horizontalLeftDir * viewRadius, Color.cyan);
            UnityEngine.Debug.DrawRay(originPos, lookDir * viewRadius, Color.green);
            UnityEngine.Debug.DrawRay(originPos, horizontalRightDir * viewRadius, Color.cyan);

            FindViewTargets();
        }
    }

    public Collider2D[] FindViewTargets()
    {
        hitTargets.Clear();

        Vector2 originPos = transform.position;
        Collider2D[] hitedTargets = Physics2D.OverlapCircleAll(originPos, viewRadius, viewTargetMask);

        foreach (Collider2D hitedTarget in hitedTargets)
        {
            Vector2 targetPos = hitedTarget.transform.position;
            Vector2 dir = (targetPos - originPos).normalized;
            Vector2 lookDir = AngleToDirZ(viewRotateZ);

            float dot = Vector2.Dot(lookDir, dir);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle <= horizontalViewHalfAngle)
            {
                RaycastHit2D rayHit = Physics2D.Raycast(originPos, dir, viewRadius, viewObstacleMask);
                if (rayHit)
                {
                    if (debugMode)
                        UnityEngine.Debug.DrawLine(originPos, rayHit.point, Color.yellow);
                }
                else if (Vector2.Distance(originPos, targetPos) <= viewRadius) // Check if target is within viewRadius
                {
                    hitTargets.Add(hitedTarget);

                    if (debugMode)
                        UnityEngine.Debug.DrawLine(originPos, targetPos, Color.red);
                }
            }
        }

        if (hitTargets.Count > 0)
            return hitTargets.ToArray();
        else
            return null;
    }






    private Vector2 AngleToDirZ(float angleInDegree)
    {
        float radian = (angleInDegree - transform.eulerAngles.z) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
    }
}