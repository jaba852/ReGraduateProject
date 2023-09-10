using UnityEngine;

public class CameraController : MonoBehaviour
{
    public DungeonData dungeonData; // DungeonData ��ũ��Ʈ�� �����ϱ� ���� ����

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

        // ���� ����� Room�� ã���ϴ�.
        Vector2Int closestRoomCenterPos = FindClosestRoomCenterPos(playerTransform.position);

        // �ش� Room�� �߽� ��ġ�� ī�޶� �̵��մϴ�.
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

            // ���� �Ÿ��� ����Ͽ� ��Ʈ ������ ȸ���մϴ�.
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
