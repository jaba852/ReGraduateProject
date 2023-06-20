using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System.Linq; // 추가


public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private int RoomSizeInt = 10;

    private Vector2Int roomSize; // 방의 크기
    private int roomInterval;

    [SerializeField]
    private Tilemap roomTile, colliderTile; // 방과 통로를 그릴 Tilemap
    [SerializeField]
    private TileBase roomFloor, corridorFloor, wall; // 방과 통로의 타일   
    [SerializeField]
    private int roomCount = 10; // 생성할 방의 개수
    [SerializeField]
    private int branchCount = 10; // 각 위치에서의 분기 수
    private DungeonData dungeonData; // 던전 데이터 객체
    public UnityEvent OnFinishedRoomGeneration; // 방 생성 완료 시 호출될 이벤트


    public static List<Vector2Int> fourDirections = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    }; // 상하좌우 방향 리스트


    private void Awake()
    {
        roomSize = new Vector2Int(RoomSizeInt, RoomSizeInt);
        roomInterval = (int)(1.5 * RoomSizeInt);
        dungeonData = FindObjectOfType<DungeonData>(); // DungeonData 컴포넌트를 찾아서 할당
    }

    private void Start()
    {
        
        Generate(); // 던전 생성 시작
    }

    private void Generate()
    {
        Vector2Int startPosition = Vector2Int.zero; // 시작 위치


        dungeonData.Rooms.Add(GenerateRectangularRoomAt(startPosition, roomSize));

        GenerateBranch(startPosition, roomCount, branchCount); // 가지를 생성

        // 생성된 방들 간에 통로를 연결
        List<Vector2Int> roomPositions = dungeonData.Rooms.Select(room => room.Position).ToList();
        for (int i = 0; i < roomPositions.Count - 1; i++)
        {
            Vector2Int start = roomPositions[i];
            Vector2Int end = roomPositions[i + 1];
            dungeonData.Path.UnionWith(CreateStraightCorridor(start, end)); // 직선 통로 생성
        }

        GenerateDungeonCollider(); // 던전의 충돌 영역 생성

        StartCoroutine(RoomFinishCo());

        OnFinishedRoomGeneration?.Invoke(); // 방 생성 완료 이벤트 호출
    }
    private IEnumerator RoomFinishCo()
    {
        yield return new WaitForSeconds(2f);
    }

    private void GenerateBranch(Vector2Int startPosition, int roomCount, int branchCount)
    {
        Stack<Vector2Int> positionStack = new Stack<Vector2Int>();
        positionStack.Push(startPosition);

        List<Vector2Int> roomPositions = new List<Vector2Int>();
        roomPositions.Add(startPosition);

        int remainingRooms = roomCount - 1; // 시작 위치를 제외한 남은 방의 개수

        while (remainingRooms > 0 && positionStack.Count > 0)
        {
            Vector2Int currentPosition = positionStack.Pop(); // 현재 위치

            // 랜덤 방향으로 분기
            List<Vector2Int> directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            ShuffleList(directions); // 방향 리스트를 무작위로 섞음

            bool roomGenerated = false; // 방이 생성되었는지 여부 확인

            for (int i = 0; i < branchCount; i++)
            {
                Vector2Int direction = directions[i % directions.Count]; // 방향 선택
                Vector2Int nextPosition = currentPosition + direction * roomInterval; // 다음 위치 계산

                if (!roomPositions.Contains(nextPosition))
                {
                    dungeonData.Rooms.Add(GenerateRectangularRoomAt(nextPosition, roomSize)); // 사각형 모양의 방 생성
                    roomPositions.Add(nextPosition);
                    positionStack.Push(nextPosition);
                    remainingRooms--;

                    // 통로 생성
                    Vector2Int start = currentPosition;
                    Vector2Int end = nextPosition;
                    dungeonData.Path.UnionWith(CreateStraightCorridor(start, end));
                    roomGenerated = true;
                }
            }

            // 분기점이 아니더라도 현재 위치에서 방을 생성
            if (!roomGenerated && remainingRooms > 0)
            {
                Vector2Int nextPosition = currentPosition + directions[0] * roomInterval; // 첫 번째 방향으로 이동

                if (!roomPositions.Contains(nextPosition))
                {
                    dungeonData.Rooms.Add(GenerateRectangularRoomAt(nextPosition, roomSize)); // 사각형 모양의 방 생성
                    roomPositions.Add(nextPosition);
                    positionStack.Push(nextPosition);
                    remainingRooms--;
                }
            }
        }
    }



    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // 방향을 랜덤으로 선택하는 함수
    private Vector2Int GetRandomDirection()
    {
        int random = UnityEngine.Random.Range(0, 4);

        switch (random)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.down;
            case 3:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }


    private DungeonData.Room GenerateRectangularRoomAt(Vector2 roomCenterPosition, Vector2Int roomSize)
    {
        Vector2Int half = roomSize / 2; // 반지름 계산

        HashSet<Vector2Int> roomTiles = new();

        for (int x = -half.x; x < half.x; x++)
        {
            for (int y = -half.y; y < half.y; y++)
            {
                Vector2 position = roomCenterPosition + new Vector2(x, y);
                Vector3Int positionInt = roomTile.WorldToCell(position);
                roomTiles.Add((Vector2Int)positionInt);
                roomTile.SetTile(positionInt, roomFloor); // 타일맵에 방의 타일 그리기
            }
        }
        return new DungeonData.Room(roomCenterPosition, roomTiles); // 방 데이터 반환
    }

    private void GenerateDungeonCollider()
    {
        HashSet<Vector2Int> dungeonTiles = new HashSet<Vector2Int>();
        foreach (DungeonData.Room room in dungeonData.Rooms)
        {
            dungeonTiles.UnionWith(room.FloorTiles);
        }
        dungeonTiles.UnionWith(dungeonData.Path);

        HashSet<Vector2Int> colliderTiles = new HashSet<Vector2Int>();
        foreach (Vector2Int tilePosition in dungeonTiles)
        {
            bool hasAdjacentTile = false;

            foreach (Vector2Int direction in fourDirections)
            {
                Vector2Int adjacentPosition = tilePosition + direction;
                if (!dungeonTiles.Contains(adjacentPosition))
                {
                    hasAdjacentTile = true;
                    break;
                }
            }

            if (hasAdjacentTile)
            {
                for (int x = tilePosition.x - 1; x <= tilePosition.x + 1; x++)
                {
                    for (int y = tilePosition.y - 1; y <= tilePosition.y + 1; y++)
                    {
                        Vector2Int newPosition = new Vector2Int(x, y);
                        if (!dungeonTiles.Contains(newPosition))
                        {
                            colliderTiles.Add(newPosition);
                        }
                    }
                }
            }
        }

        foreach (Vector2Int pos in colliderTiles)
        {
            colliderTile.SetTile((Vector3Int)pos, wall); // 타일맵에 충돌 영역의 타일 그리기
        }
    }


    private HashSet<Vector2Int> CreateStraightCorridor(Vector2Int startPostion, Vector2Int endPosition)
    {
        HashSet<Vector2Int> colliderTiles = new();
        colliderTiles.Add(startPostion);
        roomTile.SetTile((Vector3Int)startPostion, corridorFloor); // 시작 지점 타일 그리기
        colliderTiles.Add(endPosition);
        roomTile.SetTile((Vector3Int)endPosition, corridorFloor); // 끝 지점 타일 그리기

        Vector2Int direction = Vector2Int.CeilToInt(((Vector2)endPosition - startPostion).normalized); // 방향 계산
        Vector2Int currentPosition = startPostion;

        while (Vector2.Distance(currentPosition, endPosition) > 1) // 시작과 끝 지점이 1 이상 떨어져 있는 동안
        {
            currentPosition += direction;
            colliderTiles.Add(currentPosition);
            roomTile.SetTile((Vector3Int)currentPosition, corridorFloor); // 통로 타일 그리기
        }

        return colliderTiles;
    }
}
