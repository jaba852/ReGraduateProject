using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab; // �÷��̾� ������ ����

    [SerializeField]
    private List<GameObject> enemyPrefabs; // �� ������ ����Ʈ ����

    [SerializeField]
    private List<GameObject> doorObjectPrefabs; // �� ������ ����

    [SerializeField]
    private GameObject exitPortalPrefab; // Exit Portal ������ ����

    [SerializeField]
    private int playerRoomIndex; // �÷��̾ ��ġ�� ���� �ε��� ����

    [SerializeField]
    private List<int> roomEnemiesCount; // �� �濡 ��ġ�� ���� �� ����Ʈ ����

    DungeonData dungeonData; // ���� ������ ��ü

    private GameObject exitPortal; // Exit Portal ������Ʈ ����

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>(); // DungeonData ������Ʈ�� ã�Ƽ� �Ҵ�
    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
            return;

        // Exit Portal ����
        if (exitPortal != null)
        {
            Destroy(exitPortal);
        }

        foreach (Vector2Int doorPosition in dungeonData.doorPos.ToList()) // ��ġ�� ��ġ�� �����Ͽ� ��ȸ�ϸ鼭 ����
        {
            dungeonData.doorPos.Remove(doorPosition);

            if (doorObjectPrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, doorObjectPrefabs.Count);
                GameObject doorObjectPrefab = doorObjectPrefabs[randomIndex];
                GameObject doorObject = Instantiate(doorObjectPrefab);
                doorObject.transform.position = new Vector3(doorPosition.x + 0.5f, doorPosition.y + 0.5f, 0f);
            }
        }

        for (int i = 0; i < dungeonData.Rooms.Count; i++)
        {
            DungeonData.Room room = dungeonData.Rooms[i];
            RoomGraph roomGraph = new RoomGraph(room.FloorTiles);

            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>(room.FloorTiles);
            roomFloor.IntersectWith(dungeonData.Path);

            Dictionary<Vector2Int, Vector2Int> roomMap = roomGraph.RunBFS(roomFloor.First(), room.ObjectPositions);
            room.PositionsAccessibleFromPath = roomMap.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            StartCoroutine(EnemyPlacementCoroutine(room, i));
        }
    }

    public IEnumerator EnemyPlacementCoroutine(DungeonData.Room room, int i)
    {
        if (i == playerRoomIndex) // ���� ���� �÷��̾ ��ġ�� ������ Ȯ��
        {
            GameObject player = Instantiate(playerPrefab); // �÷��̾� �������� �ν��Ͻ�ȭ�Ͽ� ����
            player.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f; // �÷��̾ �� �߾ӿ� ��ġ��Ŵ
        }

        if (roomEnemiesCount.Count > i) // ���� �濡 ��ġ�� ���� ���� �ִ��� Ȯ��
        {
            PlaceEnemies(room, roomEnemiesCount[i]); // �� ��ġ
            yield return new WaitForSeconds(0.2f); // 0.2�� ���
        }

        // Exit Portal ��ġ
        if (i == dungeonData.Rooms.Count - 1) // ������ ������ Ȯ��
        {
            Vector2 centerPos = room.RoomCenterPos;
            exitPortal = Instantiate(exitPortalPrefab, new Vector3(centerPos.x + 0.5f, centerPos.y + 0.5f, 0f), Quaternion.identity);
        }
    }

    private void PlaceEnemies(DungeonData.Room room, int enemyCount)
    {
        StartCoroutine(PlaceEnemiesCoroutine(room, enemyCount));
    }

    private IEnumerator PlaceEnemiesCoroutine(DungeonData.Room room, int enemyCount)
    {
        for (int k = 0; k < enemyCount; k++)
        {
            if (room.PositionsAccessibleFromPath.Count <= k) // ���� ������ ��ġ�� ���� ���� ���� ��ġ�� �ε������� ���� ��� ����
            {
                yield break;
            }
            yield return new WaitForSeconds(.0f); // 1�� ���

            GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)]; // �� ������ �߿��� �����ϰ� ����
            GameObject enemy = Instantiate(enemyPrefab); // �� �������� �ν��Ͻ�ȭ�Ͽ� ����
            enemy.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[k] + Vector2.one * 0.5f; // ���� ���� ������ ��ġ�� ��ġ

        }
    }

}

public class RoomGraph
{
    public static List<Vector2Int> fourDirections = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>(); // �׷����� ��Ÿ���� ��ųʸ�

    public RoomGraph(HashSet<Vector2Int> roomFloor)
    {
        foreach (Vector2Int pos in roomFloor)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();

            foreach (Vector2Int direction in fourDirections)
            {
                Vector2Int newPos = pos + direction;

                if (roomFloor.Contains(newPos))
                {
                    neighbours.Add(newPos); // ������ Ÿ���̸� neighbors ����Ʈ�� �߰�
                }
            }

            graph.Add(pos, neighbours); // �׷����� ���� Ÿ�ϰ� ������ Ÿ�ϵ��� �߰�
        }
    }

    public Dictionary<Vector2Int, Vector2Int> RunBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
    {
        Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(startPos);

        Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();
        map.Add(startPos, startPos);

        while (nodesToVisit.Count > 0)
        {
            Vector2Int node = nodesToVisit.Dequeue();
            List<Vector2Int> neighbours = graph[node];

            foreach (Vector2Int neighbourPosition in neighbours)
            {
                if (visitedNodes.Contains(neighbourPosition) == false &&
                    occupiedNodes.Contains(neighbourPosition) == false)
                {
                    nodesToVisit.Enqueue(neighbourPosition);
                    visitedNodes.Add(neighbourPosition);
                    map[neighbourPosition] = node; // BFS�� ����Ͽ� ���� ������ ��ġ�� ã�Ƽ� �ʿ� �߰�
                }
            }
        }

        return map;
    }
}
