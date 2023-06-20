using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class ObjectPlacement : MonoBehaviour
{
    DungeonData dungeonData;

    [SerializeField]
    private List<ObjectData> objectsToPlace;

    [SerializeField, Range(0, 1)]
    private float cornerObjectPlacementChance = 0.7f;

    [SerializeField]
    private GameObject objectPrefab;

    public UnityEvent OnFinished;

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
            PlaceAllObject(room);
        }
        StartCoroutine(RoomOBJFinishCo());
        OnFinished?.Invoke();

    }
    private IEnumerator RoomOBJFinishCo()
    {
        yield return new WaitForSeconds(2f);
    }

    private void PlaceAllObject(DungeonData.Room room)
    {
        List<ObjectData> cornerProps = objectsToPlace.Where(x => x.Corner).ToList();
        PlaceCornerObjects(room, cornerProps);

        List<ObjectData> leftWallProps = objectsToPlace.Where(x => x.NearWallLeft).OrderByDescending(x => x.ObjectSize.x * x.ObjectSize.y).ToList();
        PlaceObjects(room, leftWallProps, room.NearWallTilesLeft, PlacementOriginCorner.BottomLeft);

        List<ObjectData> rightWallProps = objectsToPlace.Where(x => x.NearWallRight).OrderByDescending(x => x.ObjectSize.x * x.ObjectSize.y).ToList();
        PlaceObjects(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.TopRight);

        List<ObjectData> topWallProps = objectsToPlace.Where(x => x.NearWallUP).OrderByDescending(x => x.ObjectSize.x * x.ObjectSize.y).ToList();
        PlaceObjects(room, topWallProps, room.NearWallTilesUp, PlacementOriginCorner.TopLeft);

        List<ObjectData> downWallProps = objectsToPlace.Where(x => x.NearWallDown).OrderByDescending(x => x.ObjectSize.x * x.ObjectSize.y).ToList();
        PlaceObjects(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.BottomLeft);

        List<ObjectData> innerProps = objectsToPlace.Where(x => x.Inner).OrderByDescending(x => x.ObjectSize.x * x.ObjectSize.y).ToList();
        PlaceObjects(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);
    }
    public void RunEvent()
    {
        OnFinished?.Invoke();
    }
    private void PlaceObjects(DungeonData.Room room, List<ObjectData> wallProps, HashSet<Vector2Int> availableTiles, PlacementOriginCorner placement)
    {
        HashSet<Vector2Int> tempPositions = new HashSet<Vector2Int>(availableTiles);
        tempPositions.ExceptWith(dungeonData.Path);

        foreach (ObjectData objectToPlace in wallProps)
        {
            int quantity = UnityEngine.Random.Range(objectToPlace.PlacementQuantityMin, objectToPlace.PlacementQuantityMax + 1);

            for (int i = 0; i < quantity; i++)
            {
                tempPositions.ExceptWith(room.ObjectPositions);

                List<Vector2Int> availablePositions = tempPositions.OrderBy(x => Guid.NewGuid()).ToList();

                List<ObjectData> weightedObjects = new List<ObjectData>();
                for (int j = 0; j < objectToPlace.PlacementWeight; j++)
                {
                    weightedObjects.Add(objectToPlace);
                }

                foreach (ObjectData selectedObject in weightedObjects)
                {
                    // 0부터 1 사이의 랜덤 값을 생성합니다.
                    float randomValue = UnityEngine.Random.value;

                    // 랜덤 값이 선택한 오브젝트의 배치 확률보다 작거나 같으면 배치합니다.
                    if (randomValue <= selectedObject.PlacementWeight)
                    {
                        if (TryPlacingObjectBruteForce(room, selectedObject, availablePositions, placement) == false)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }


    private bool TryPlacingObjectBruteForce(DungeonData.Room room, ObjectData objectToPlace, List<Vector2Int> availablePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < availablePositions.Count; i++)
        {
            Vector2Int position = availablePositions[i];
            if (room.ObjectPositions.Contains(position))
            {
                continue;
            }
            List<Vector2Int> freePositionsAround = TryToFitProp(objectToPlace, availablePositions, position, placement);

            if (freePositionsAround.Count == objectToPlace.ObjectSize.x * objectToPlace.ObjectSize.y)
            {
                PlaceObjectGameObjectAt(room, position, objectToPlace);
                foreach (Vector2Int pos in freePositionsAround)
                {
                    room.ObjectPositions.Add(pos);
                }
                if (objectToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, position, objectToPlace, 1);
                }
                return true;
            }
        }
        return false;
    }

    private List<Vector2Int> TryToFitProp(ObjectData objectData, List<Vector2Int> availablePositions, Vector2Int originPosition, PlacementOriginCorner placement)
    {
        List<Vector2Int> freePositions = new List<Vector2Int>();

        if (placement == PlacementOriginCorner.BottomLeft)
        {
            for (int xOffset = 0; xOffset < objectData.ObjectSize.x; xOffset++)
            {
                for (int yOffset = 0; yOffset < objectData.ObjectSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                    {
                        freePositions.Add(tempPos);
                    }
                }
            }
        }
        else if (placement == PlacementOriginCorner.BottomRight)
        {
            for (int xOffset = -objectData.ObjectSize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = 0; yOffset < objectData.ObjectSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                    {
                        freePositions.Add(tempPos);
                    }
                }
            }
        }
        else if (placement == PlacementOriginCorner.TopLeft)
        {
            for (int xOffset = 0; xOffset < objectData.ObjectSize.x; xOffset++)
            {
                for (int yOffset = -objectData.ObjectSize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                    {
                        freePositions.Add(tempPos);
                    }
                }
            }
        }
        else
        {
            for (int xOffset = -objectData.ObjectSize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = -objectData.ObjectSize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if (availablePositions.Contains(tempPos))
                    {
                        freePositions.Add(tempPos);
                    }
                }
            }
        }

        return freePositions;
    }

    private void PlaceCornerObjects(DungeonData.Room room, List<ObjectData> cornerObjects)
    {
        float tempChance = cornerObjectPlacementChance;

        foreach (Vector2Int cornerTile in room.CornerTiles)
        {
            if (UnityEngine.Random.value < tempChance)
            {
                ObjectData propToPlace = cornerObjects[UnityEngine.Random.Range(0, cornerObjects.Count)];

                PlaceObjectGameObjectAt(room, cornerTile, propToPlace);
                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, cornerTile, propToPlace, 2);
                }
            }
            else
            {
                tempChance = Mathf.Clamp01(tempChance + 0.1f);
            }
        }
    }

    private GameObject PlaceObjectGameObjectAt(DungeonData.Room room, Vector2Int placementPosition, ObjectData objectData)
    {
        GameObject objectInstance = Instantiate(objectPrefab);
        SpriteRenderer propSpriteRenderer = objectInstance.GetComponentInChildren<SpriteRenderer>();

        propSpriteRenderer.sprite = objectData.ObjectSprite;

        CapsuleCollider2D collider = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
        if (objectData.isLight)
        {
            Light2D light2D = propSpriteRenderer.gameObject.AddComponent<Light2D>();

        }

        collider.offset = Vector2.zero;
        if (objectData.ObjectSize.x > objectData.ObjectSize.y)
        {
            collider.direction = CapsuleDirection2D.Horizontal;
        }
        Vector2 size = new Vector2(objectData.ObjectSize.x * 0.8f, objectData.ObjectSize.y * 0.8f);
        collider.size = size;
        objectInstance.transform.localPosition = (Vector2)placementPosition;
                
        propSpriteRenderer.transform.localPosition = (Vector2)objectData.ObjectSize * 0.5f;

        room.ObjectPositions.Add(placementPosition);
        room.ObjectReferences.Add(objectInstance);


        return objectInstance;
    }

    private void PlaceGroupObject(DungeonData.Room room, Vector2Int groupOriginPosition, ObjectData objectData, int searchOffset)
    {
        int count = UnityEngine.Random.Range(objectData.GroupMinCount, objectData.GroupMaxCount) - 1;
        count = Mathf.Clamp(count, 0, 8);

        List<Vector2Int> availableSpaces = new List<Vector2Int>();
        for (int xOffset = -searchOffset; xOffset <= searchOffset; xOffset++)
        {
            for (int yOffset = -searchOffset; yOffset <= searchOffset; yOffset++)
            {
                Vector2Int tempPos = groupOriginPosition + new Vector2Int(xOffset, yOffset);
                if (room.FloorTiles.Contains(tempPos) &&
                    !dungeonData.Path.Contains(tempPos) &&
                    !room.ObjectPositions.Contains(tempPos))
                {
                    availableSpaces.Add(tempPos);
                }
            }
        }
    }





}
public enum PlacementOriginCorner
{
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight
}
