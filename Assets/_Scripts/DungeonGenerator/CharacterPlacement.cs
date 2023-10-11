using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab; // 플레이어 프리팹 변수

    [SerializeField]
    private List<GameObject> enemyPrefabs; // 적 프리팹 리스트 변수

    [SerializeField]
    private List<GameObject> doorObjectPrefabs; // 문 프리팹 변수

    [SerializeField]
    private GameObject exitPortalPrefab; // Exit Portal 프리팹 변수

    [SerializeField]
    private GameObject itemBoxPrefab; // ItemBox 프리팹 변수

    [SerializeField]
    private int playerRoomIndex; // 플레이어가 위치할 방의 인덱스 변수

    [SerializeField]
    private List<int> roomEnemiesCount; // 각 방에 배치될 적의 수 리스트 변수

    DungeonData dungeonData; // 던전 데이터 객체

    [SerializeField]
    private bool showGizmo = false; // Gizmo 표시 여부 변수

    private GameObject exitPortal; // Exit Portal 오브젝트 변수
    //추가된 사항
    [SerializeField]
    private List<GameObject> simpleDoorObjectTop; // 상단 door 프리팹 변수

    [SerializeField]
    private List<GameObject> simpleDoorObjectBottom; // 하단 door 프리팹 변수

    [SerializeField]
    private List<GameObject> simpleDoorObjectLeft; // 좌측 door 프리팹 변수

    [SerializeField]
    private List<GameObject> simpleDoorObjectRight; // 우측 door 프리팹 변수
    //여기까지
    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>(); // DungeonData 컴포넌트를 찾아서 할당
    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
            return;

        // Exit Portal 삭제
        if (exitPortal != null)
        {
            Destroy(exitPortal);
        }

        foreach (Vector2Int doorPosition in dungeonData.topDoorPos.ToList()) // 배치된 위치를 복사하여 순회하면서 제거
        {
            dungeonData.doorPos.Remove(doorPosition);

            if (doorObjectPrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, doorObjectPrefabs.Count);
                GameObject doorObjectPrefab = simpleDoorObjectTop[randomIndex];
                GameObject doorObject = Instantiate(doorObjectPrefab);
                doorObject.transform.position = new Vector3(doorPosition.x + 0.5f, doorPosition.y - 0.2f, 0f);
            }

        }
        foreach (Vector2Int doorPosition in dungeonData.bottomDoorPos.ToList()) // 배치된 위치를 복사하여 순회하면서 제거
        {
            dungeonData.doorPos.Remove(doorPosition);

            if (doorObjectPrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, doorObjectPrefabs.Count);
                GameObject doorObjectPrefab = simpleDoorObjectBottom[randomIndex];
                GameObject doorObject = Instantiate(doorObjectPrefab);
                doorObject.transform.position = new Vector3(doorPosition.x + 0.5f, doorPosition.y + 0.5f, 0f);
            }


        }
        foreach (Vector2Int doorPosition in dungeonData.leftDoorPos.ToList()) // 배치된 위치를 복사하여 순회하면서 제거
        {
            dungeonData.doorPos.Remove(doorPosition);

            if (doorObjectPrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, doorObjectPrefabs.Count);
                GameObject doorObjectPrefab = simpleDoorObjectLeft[randomIndex];
                GameObject doorObject = Instantiate(doorObjectPrefab);
                doorObject.transform.position = new Vector3(doorPosition.x + 1.0f, doorPosition.y + 0.3f, 0f);
            }


        }
        foreach (Vector2Int doorPosition in dungeonData.rightDoorPos.ToList()) // 배치된 위치를 복사하여 순회하면서 제거
        {
            dungeonData.doorPos.Remove(doorPosition);

            if (doorObjectPrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, doorObjectPrefabs.Count);
                GameObject doorObjectPrefab = simpleDoorObjectRight[randomIndex];
                GameObject doorObject = Instantiate(doorObjectPrefab);
                doorObject.transform.position = new Vector3(doorPosition.x - 0.2f, doorPosition.y + 0.3f, 0f);
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

        if (i == playerRoomIndex) // 현재 방이 플레이어가 위치할 방인지 확인
        {
            GameObject player = Instantiate(playerPrefab); // 플레이어 프리팹을 인스턴스화하여 생성
            player.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f; // 플레이어를 방 중앙에 위치시킴
        }

        if (roomEnemiesCount.Count > i) // 현재 방에 배치할 적의 수가 있는지 확인
        {
            PlaceEnemies(room, roomEnemiesCount[i]); // 적 배치
            yield return new WaitForSeconds(0.2f); // 0.2초 대기
        }

        // Item Box 배치
        if (i == 4) // 마지막 방인지 확인
        {
            Vector2 centerPos = room.RoomCenterPos;
            GameObject ItemBox = Instantiate(itemBoxPrefab);
            ItemBox.transform.position = new Vector3(centerPos.x + 0.5f, centerPos.y + 0.5f, 0f);
        }

        // Exit Portal 배치
        if (i == dungeonData.Rooms.Count - 1) // 마지막 방인지 확인
        {
            Vector2 centerPos = room.RoomCenterPos;
            exitPortal = Instantiate(exitPortalPrefab, new Vector3(centerPos.x + 0.5f, centerPos.y + 0.5f, 0f), Quaternion.identity);
        }

    }

    private void PlaceEnemies(DungeonData.Room room, int enemyCount)
    {
        for (int k = 0; k < enemyCount; k++)
        {
            if (room.PositionsAccessibleFromPath.Count <= k) // 접근 가능한 위치의 수가 현재 적을 배치할 인덱스보다 작은 경우 종료
            {
                return;
            }

            GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)]; // 적 프리팹 중에서 랜덤하게 선택
            GameObject enemy = Instantiate(enemyPrefab); // 적 프리팹을 인스턴스화하여 생성
            enemy.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[k] + Vector2.one * 0.5f; // 적을 접근 가능한 위치에 배치
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (dungeonData == null || showGizmo == false) // 던전 데이터가 없거나 Gizmo 표시가 비활성화된 경우
        {
            Debug.Log("오류발생");
            return;
        }

        foreach (DungeonData.Room room in dungeonData.Rooms)
        {
            Color color = Color.green;
            color.a = 0.3f;
            Gizmos.color = color;

            foreach (Vector2Int pos in room.PositionsAccessibleFromPath)
            {
                Gizmos.DrawCube((Vector2)pos + Vector2.one * 0.5f, Vector2.one); // 접근 가능한 위치에 사각형 형태의 Gizmo를 그림
            }
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

    Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>(); // 그래프를 나타내는 딕셔너리

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
                    neighbours.Add(newPos); // 인접한 타일이면 neighbors 리스트에 추가
                }
            }

            graph.Add(pos, neighbours); // 그래프에 현재 타일과 인접한 타일들을 추가
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
                    map[neighbourPosition] = node; // BFS를 사용하여 접근 가능한 위치를 찾아서 맵에 추가
                }
            }
        }

        return map;
    }
}