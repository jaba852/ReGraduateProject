using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System.Linq; // �߰�


public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private int RoomSizeInt = 10;

    private Vector2Int roomSize; // ���� ũ��
    private int roomInterval;

    [SerializeField]
    private Tilemap roomTile, colliderTile; // ��� ��θ� �׸� Tilemap
    [SerializeField]
    private TileBase roomFloor, corridorFloor, wall; // ��� ����� Ÿ��   
    [SerializeField]
    private int roomCount = 10; // ������ ���� ����
    [SerializeField]
    private int branchCount = 10; // �� ��ġ������ �б� ��
    private DungeonData dungeonData; // ���� ������ ��ü
    public UnityEvent OnFinishedRoomGeneration; // �� ���� �Ϸ� �� ȣ��� �̺�Ʈ


    public static List<Vector2Int> fourDirections = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    }; // �����¿� ���� ����Ʈ


    private void Awake()
    {
        roomSize = new Vector2Int(RoomSizeInt, RoomSizeInt);
        roomInterval = (int)(1.5 * RoomSizeInt);
        dungeonData = FindObjectOfType<DungeonData>(); // DungeonData ������Ʈ�� ã�Ƽ� �Ҵ�
    }

    private void Start()
    {
        
        Generate(); // ���� ���� ����
    }

    private void Generate()
    {
        Vector2Int startPosition = Vector2Int.zero; // ���� ��ġ


        dungeonData.Rooms.Add(GenerateRectangularRoomAt(startPosition, roomSize));

        GenerateBranch(startPosition, roomCount, branchCount); // ������ ����

        // ������ ��� ���� ��θ� ����
        List<Vector2Int> roomPositions = dungeonData.Rooms.Select(room => room.Position).ToList();
        for (int i = 0; i < roomPositions.Count - 1; i++)
        {
            Vector2Int start = roomPositions[i];
            Vector2Int end = roomPositions[i + 1];
            dungeonData.Path.UnionWith(CreateStraightCorridor(start, end)); // ���� ��� ����
        }

        GenerateDungeonCollider(); // ������ �浹 ���� ����

        StartCoroutine(RoomFinishCo());

        OnFinishedRoomGeneration?.Invoke(); // �� ���� �Ϸ� �̺�Ʈ ȣ��
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

        int remainingRooms = roomCount - 1; // ���� ��ġ�� ������ ���� ���� ����

        while (remainingRooms > 0 && positionStack.Count > 0)
        {
            Vector2Int currentPosition = positionStack.Pop(); // ���� ��ġ

            // ���� �������� �б�
            List<Vector2Int> directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            ShuffleList(directions); // ���� ����Ʈ�� �������� ����

            bool roomGenerated = false; // ���� �����Ǿ����� ���� Ȯ��

            for (int i = 0; i < branchCount; i++)
            {
                Vector2Int direction = directions[i % directions.Count]; // ���� ����
                Vector2Int nextPosition = currentPosition + direction * roomInterval; // ���� ��ġ ���

                if (!roomPositions.Contains(nextPosition))
                {
                    dungeonData.Rooms.Add(GenerateRectangularRoomAt(nextPosition, roomSize)); // �簢�� ����� �� ����
                    roomPositions.Add(nextPosition);
                    positionStack.Push(nextPosition);
                    remainingRooms--;

                    // ��� ����
                    Vector2Int start = currentPosition;
                    Vector2Int end = nextPosition;
                    dungeonData.Path.UnionWith(CreateStraightCorridor(start, end));
                    roomGenerated = true;
                }
            }

            // �б����� �ƴϴ��� ���� ��ġ���� ���� ����
            if (!roomGenerated && remainingRooms > 0)
            {
                Vector2Int nextPosition = currentPosition + directions[0] * roomInterval; // ù ��° �������� �̵�

                if (!roomPositions.Contains(nextPosition))
                {
                    dungeonData.Rooms.Add(GenerateRectangularRoomAt(nextPosition, roomSize)); // �簢�� ����� �� ����
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

    // ������ �������� �����ϴ� �Լ�
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
        Vector2Int half = roomSize / 2; // ������ ���

        HashSet<Vector2Int> roomTiles = new();

        for (int x = -half.x; x < half.x; x++)
        {
            for (int y = -half.y; y < half.y; y++)
            {
                Vector2 position = roomCenterPosition + new Vector2(x, y);
                Vector3Int positionInt = roomTile.WorldToCell(position);
                roomTiles.Add((Vector2Int)positionInt);
                roomTile.SetTile(positionInt, roomFloor); // Ÿ�ϸʿ� ���� Ÿ�� �׸���
            }
        }
        return new DungeonData.Room(roomCenterPosition, roomTiles); // �� ������ ��ȯ
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
            colliderTile.SetTile((Vector3Int)pos, wall); // Ÿ�ϸʿ� �浹 ������ Ÿ�� �׸���
        }
    }


    private HashSet<Vector2Int> CreateStraightCorridor(Vector2Int startPostion, Vector2Int endPosition)
    {
        HashSet<Vector2Int> colliderTiles = new();
        colliderTiles.Add(startPostion);
        roomTile.SetTile((Vector3Int)startPostion, corridorFloor); // ���� ���� Ÿ�� �׸���
        colliderTiles.Add(endPosition);
        roomTile.SetTile((Vector3Int)endPosition, corridorFloor); // �� ���� Ÿ�� �׸���

        Vector2Int direction = Vector2Int.CeilToInt(((Vector2)endPosition - startPostion).normalized); // ���� ���
        Vector2Int currentPosition = startPostion;

        while (Vector2.Distance(currentPosition, endPosition) > 1) // ���۰� �� ������ 1 �̻� ������ �ִ� ����
        {
            currentPosition += direction;
            colliderTiles.Add(currentPosition);
            roomTile.SetTile((Vector3Int)currentPosition, corridorFloor); // ��� Ÿ�� �׸���
        }

        return colliderTiles;
    }
}
