using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    public int dungeonWidth, dungeonHeight;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField]
    private EnemyFactory enemyFactory;
    [SerializeField]
    private Transform enemiesParent;
    [SerializeField]
    private CoinFactory CoinFactory;
    [SerializeField]
    private Transform coinsParent;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject boss;
    [SerializeField]
    private GameObject doorPrefab;
    [SerializeField]
    public Transform DoorParent;

    private List<GameObject> enemyInstances = new List<GameObject>();
    private List<GameObject> coinInstances = new List<GameObject>();
    private List<GameObject> doorInstances = new List<GameObject>();


  protected override void RunProceduralGeneration()
{
    List<BoundsInt> rooms = CreateRooms(out HashSet<Vector2Int> corridors);
    GenerateEnemiesInRooms(rooms);
    GenerateCoinsInRooms(rooms);
    PositionPlayerInRoom(rooms);
    PositionBossInRoom(rooms);
    GenerateDoorsInBossCorridors(rooms, corridors);
}


    private List<BoundsInt> CreateRooms(out HashSet<Vector2Int> corridors)
{
    var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
    floor = CreateSimpleRooms(roomsList);
    List<Vector2Int> roomCenters = new List<Vector2Int>();
    foreach (var room in roomsList)
    {
        roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
    }
    corridors = ConnectRooms(roomCenters);
    corridors = IncreaseCorridorSize(corridors);
    floor.UnionWith(corridors);
    tilemapVisualizer.PaintFloorTiles(floor);
    WallGenerator.CreateWalls(floor, tilemapVisualizer);
    return roomsList;
}


    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        for (int i = 0; i < roomCenters.Count; i++)
        {
            var currentRoom = roomCenters[i];
            Vector2Int closestRoom1 = FindClosestPointTo(currentRoom, roomCenters, new List<Vector2Int> { currentRoom });
            Vector2Int closestRoom2 = FindClosestPointTo(currentRoom, roomCenters, new List<Vector2Int> { currentRoom, closestRoom1 });

            HashSet<Vector2Int> newCorridor1 = CreateCorridor(currentRoom, closestRoom1);
            HashSet<Vector2Int> newCorridor2 = CreateCorridor(currentRoom, closestRoom2);

            corridors.UnionWith(newCorridor1);
            corridors.UnionWith(newCorridor2);
        }

        return corridors;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters, List<Vector2Int> exclude)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            if (exclude.Contains(position)) continue;
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private void GenerateEnemiesInRooms(List<BoundsInt> rooms)
    {
        foreach (var enemyInstance in enemyInstances)
        {
            DestroyImmediate(enemyInstance);
        }
        enemyInstances.Clear();

        foreach (var roomBounds in rooms)
        {
            GenerateEnemyInRoom(roomBounds);
        }
    }

    private void GenerateEnemyInRoom(BoundsInt roomBounds)
    {
        int numberOfEnemies = Random.Range(1, 5);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(roomBounds.min.x + 5, roomBounds.max.x - 5), Random.Range(roomBounds.min.y + 5, roomBounds.max.y - 5), 0);
            GameObject newEnemy = enemyFactory.CreateEnemy(randomPosition, enemiesParent);
            enemyInstances.Add(newEnemy);
        }
    }

    private void GenerateCoinsInRooms(List<BoundsInt> rooms)
    {
        foreach (var coinInstance in coinInstances)
        {
            DestroyImmediate(coinInstance);
        }
        coinInstances.Clear();

        foreach (var roomBounds in rooms)
        {
            GenerateCoinInRoom(roomBounds);
        }
    }

   private void GenerateCoinInRoom(BoundsInt roomBounds)
{
    int numberOfCoins = Random.Range(1, 20);
    for (int i = 0; i < numberOfCoins; i++)
    {
        Vector3 randomPosition = new Vector3(Random.Range(roomBounds.min.x + 5, roomBounds.max.x - 5), Random.Range(roomBounds.min.y + 5, roomBounds.max.y - 5), 0);
        GameObject newCoin = CoinFactory.CreateCoin(randomPosition, coinsParent);
        coinInstances.Add(newCoin);

        // Ajouter la pièce à la liste du CoinManager
        CoinManager cm = FindObjectOfType<CoinManager>();
        if (cm != null)
        {
            cm.coins.Add(newCoin);
        }
    }
}

    private HashSet<Vector2Int> IncreaseCorridorSize(HashSet<Vector2Int> corridors)
    {
        HashSet<Vector2Int> enlargedCorridors = new HashSet<Vector2Int>();
        foreach (var corridorTile in corridors)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    enlargedCorridors.Add(corridorTile + new Vector2Int(x, y));
                }
            }
        }
        return enlargedCorridors;
    }

    private void PositionPlayerInRoom(List<BoundsInt> rooms)
    {
        if (rooms.Count > 0)
        {
            BoundsInt startingRoom = rooms[0];
            Vector3 playerStartPos = new Vector3(startingRoom.center.x, startingRoom.center.y, 0);
            player.transform.position = playerStartPos;
        }
    }

    private void PositionBossInRoom(List<BoundsInt> rooms)
    {
        if (rooms.Count > 0)
        {
            BoundsInt bossRoom = rooms[rooms.Count - 1];
            Vector3 bossPos = new Vector3(bossRoom.center.x, bossRoom.center.y, 0);
            boss.transform.position = bossPos;
        }
    }

    
private void GenerateDoorsInBossCorridors(List<BoundsInt> rooms, HashSet<Vector2Int> corridors)
{
    // Détruire les anciennes portes
    DestroyOldDoors();

    // Générer les nouvelles portes
    if (rooms.Count > 0)
    {
        BoundsInt bossRoom = rooms[rooms.Count - 1];
        List<Vector2Int> doorPositions = FindAllCorridorsAdjacentToRoom(bossRoom, corridors);
        foreach (var doorPosition in doorPositions)
        {
            GameObject newDoor = Instantiate(doorPrefab, new Vector3(doorPosition.x, doorPosition.y, 0), Quaternion.identity,DoorParent);
            newDoor.tag = "Door"; // Assigner le tag "Door" aux nouvelles portes
            doorInstances.Add(newDoor);
        }
    }
}


  
  
  private List<Vector2Int> FindAllCorridorsAdjacentToRoom(BoundsInt room, HashSet<Vector2Int> corridors)
{
    List<Vector2Int> adjacentCorridors = new List<Vector2Int>();

    foreach (var corridor in corridors)
    {
        if (IsCorridorAdjacentToRoom(room, corridor))
        {
            adjacentCorridors.Add(corridor);
        }
    }

    return adjacentCorridors;
}

private bool IsCorridorAdjacentToRoom(BoundsInt room, Vector2Int corridor)
{
    return (corridor.x == room.xMin - 1 || corridor.x == room.xMax + 1) && (corridor.y >= room.yMin && corridor.y <= room.yMax) ||
           (corridor.y == room.yMin - 1 || corridor.y == room.yMax + 1) && (corridor.x >= room.xMin && corridor.x <= room.xMax);
}
private void DestroyOldDoors()
{
    foreach (var doorInstance in doorInstances)
    {
        if (doorInstance != null)
        {
            DestroyImmediate(doorInstance);
        }
    }
    doorInstances.Clear();
}


}
