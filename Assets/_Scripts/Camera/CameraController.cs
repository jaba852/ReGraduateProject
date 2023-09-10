using UnityEngine;

public class CameraController : MonoBehaviour
{
    public DungeonData dungeonData; // DungeonData 스크립트에 접근하기 위한 변수

    private Transform playerTransform;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // 가장 가까운 Room을 찾습니다.
        Vector2Int closestRoomCenterPos = FindClosestRoomCenterPos(playerTransform.position);

        // 해당 Room의 중심 위치로 카메라를 이동합니다.
        Vector3 cameraPosition = new Vector3(closestRoomCenterPos.x, closestRoomCenterPos.y, transform.position.z);
        transform.position = cameraPosition;
    }

    private Vector2Int FindClosestRoomCenterPos(Vector3 playerPosition)
    {
        Vector2Int closestRoomCenterPos = Vector2Int.zero;
        float closestDistanceSqr = Mathf.Infinity;

        Vector2 playerPos2D = new Vector2(playerPosition.x, playerPosition.y);

        foreach (DungeonData.Room room in dungeonData.Rooms)
        {
            Vector2 roomCenterPos = room.RoomCenterPos;
            Vector2Int roomCenterPosInt = new Vector2Int((int)roomCenterPos.x, (int)roomCenterPos.y);

            // 제곱 거리를 계산하여 루트 연산을 회피합니다.
            Vector2 delta = roomCenterPos - playerPos2D;
            float distanceSqr = delta.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestRoomCenterPos = roomCenterPosInt;
            }
        }

        return closestRoomCenterPos;
    }
}
