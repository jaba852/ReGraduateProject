using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : MonoBehaviour
{

    public List<Room> Rooms { get; set; } = new List<Room>();
    public HashSet<Vector2Int> Path { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> doorPos { get; private set; } = new HashSet<Vector2Int>();

    public class Room
    {

        public Vector2Int Position { get; set; }
        public Vector2 RoomCenterPos { get; set; }
        public HashSet<Vector2Int> FloorTiles { get; private set; } = new HashSet<Vector2Int>();

        public HashSet<Vector2Int> NearWallTilesUp { get; set; } = new HashSet<Vector2Int>();
        public HashSet<Vector2Int> NearWallTilesDown { get; set; } = new HashSet<Vector2Int>();
        public HashSet<Vector2Int> NearWallTilesLeft { get; set; } = new HashSet<Vector2Int>();
        public HashSet<Vector2Int> NearWallTilesRight { get; set; } = new HashSet<Vector2Int>();
        public HashSet<Vector2Int> CornerTiles { get; set; } = new HashSet<Vector2Int>();
        public HashSet<Vector2Int> InnerTiles { get; set; } = new HashSet<Vector2Int>();
        public HashSet<Vector2Int> ObjectPositions { get; set; } = new HashSet<Vector2Int>();
        public List<Vector2Int> PositionsAccessibleFromPath { get; set; } = new List<Vector2Int>();

        public List<GameObject> ObjectReferences { get; set; } = new List<GameObject>();
        public Room(Vector2 roomCenterPos, HashSet<Vector2Int> floorTiles)
        {
            this.RoomCenterPos = roomCenterPos;
            this.FloorTiles = floorTiles;
        }
    }

 

}
