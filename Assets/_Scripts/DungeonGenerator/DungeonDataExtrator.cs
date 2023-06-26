using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonDataExtrator : MonoBehaviour
{
    private DungeonData dungeonData;

    [SerializeField]
    private bool Gizmo = false;


    public UnityEvent OnFinishedRoomProcessing;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
    }

    public void ProcessRooms()
    {
        if (dungeonData == null)
        {
            return;
        }

        foreach (DungeonData.Room room in dungeonData.Rooms)
        {
            foreach (Vector2Int tilePosition in room.FloorTiles)
            {
                int neighboursCount = 4;

                if (room.FloorTiles.Contains(tilePosition + Vector2Int.up) == false)
                {
                    room.NearWallTilesUp.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.right) == false)
                {
                    room.NearWallTilesRight.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.down) == false)
                {
                    room.NearWallTilesDown.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.left) == false)
                {
                    room.NearWallTilesLeft.Add(tilePosition);
                    neighboursCount--;
                }

                if (neighboursCount <= 2)
                {
                    room.CornerTiles.Add(tilePosition);
                }
                if (neighboursCount == 4)
                {
                    room.InnerTiles.Add(tilePosition);
                }
            }

            room.NearWallTilesUp.ExceptWith(room.CornerTiles);
            room.NearWallTilesRight.ExceptWith(room.CornerTiles);
            room.NearWallTilesDown.ExceptWith(room.CornerTiles);
            room.NearWallTilesLeft.ExceptWith(room.CornerTiles);



            foreach (Vector2Int doorPosition in dungeonData.Path)
            {
                if (room.NearWallTilesUp.Contains(doorPosition + Vector2Int.left))
                {
                    dungeonData.doorPos.Add(doorPosition);
                }
                if (room.NearWallTilesDown.Contains(doorPosition + Vector2Int.left))
                {
                    dungeonData.doorPos.Add(doorPosition);
                }
                if (room.NearWallTilesLeft.Contains(doorPosition + Vector2Int.up))
                {
                    dungeonData.doorPos.Add(doorPosition);
                }
                if (room.NearWallTilesRight.Contains(doorPosition + Vector2Int.up))
                {
                    dungeonData.doorPos.Add(doorPosition);
                }

            }
            dungeonData.Path.ExceptWith(dungeonData.doorPos);
            room.NearWallTilesUp.ExceptWith(dungeonData.doorPos);
            room.NearWallTilesRight.ExceptWith(dungeonData.doorPos);
            room.NearWallTilesDown.ExceptWith(dungeonData.doorPos);
            room.NearWallTilesLeft.ExceptWith(dungeonData.doorPos);


        }
        OnFinishedRoomProcessing?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (dungeonData == null || Gizmo == false)
        {
            return;
        }
        foreach (DungeonData.Room room in dungeonData.Rooms)
        {
            Gizmos.color = Color.white;
            foreach (Vector2Int floorPosition in room.InnerTiles)
            {
                if (dungeonData.Path.Contains(floorPosition))
                {
                    continue;
                }
                Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            Gizmos.color = Color.red;
            foreach (Vector2Int floorPosition in room.NearWallTilesUp)
            {
                if (dungeonData.Path.Contains(floorPosition))
                {
                    continue;
                }
                Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            Gizmos.color = Color.blue;
            foreach (Vector2Int floorPosition in room.NearWallTilesDown)
            {
                if (dungeonData.Path.Contains(floorPosition))
                {
                    continue;
                }
                Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            Gizmos.color = Color.yellow;
            foreach (Vector2Int floorPosition in room.NearWallTilesRight)
            {
                if (dungeonData.Path.Contains(floorPosition))
                {
                    continue;
                }
                Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            Gizmos.color = Color.green;
            foreach (Vector2Int floorPosition in room.NearWallTilesLeft)
            {
                if (dungeonData.Path.Contains(floorPosition))
                {
                    continue;
                }
                Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            Gizmos.color = Color.black;
            foreach (Vector2Int floorPosition in room.CornerTiles)
            {
                if (dungeonData.Path.Contains(floorPosition))
                {
                    continue;
                }
                Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
        }
        // dungeonData.Path에 있는 위치만 보라색 기즈모로 그리기
        Gizmos.color = Color.magenta;
        foreach (Vector2Int pathPosition in dungeonData.Path)
        {
            Gizmos.DrawCube(pathPosition + Vector2.one * 0.5f, Vector2.one);
        }
        Gizmos.color = Color.gray;
        foreach (Vector2Int doorPosition in dungeonData.doorPos) // 문예정 위치 기즈모
        {
            Gizmos.DrawCube(doorPosition + Vector2.one * 0.5f, Vector2.one);
        }
    }
}
