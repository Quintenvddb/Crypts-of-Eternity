using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int[,] grid;
    private int gridWidth, gridHeight;
    public List<Room> Rooms { get; private set; }

    // Store spawned objects
    public List<GameObject> spawnedObjects { get; private set; } = new List<GameObject>();

    // Prefabs for Enemy, Loot, and Shop
    public List<GameObject> enemyPrefabs;
    public GameObject lootPrefab;
    public GameObject shopPrefab;
    public GameObject trapPrefab;

    // Counters for spawned objects
    private int enemyCount = 0;
    private int lootCount = 0;
    private int shopCount = 0;
    private int trapCount = 0;

    // Parent GameObjects for organizing spawned objects
    private GameObject enemyParent;
    private GameObject lootParent;
    private GameObject shopParent;
    private GameObject trapParent;

    public RoomGenerator(int[,] grid, int width, int height)
    {
        this.grid = grid;
        this.gridWidth = width;
        this.gridHeight = height;
        Rooms = new List<Room>();

        // Create parent objects in the hierarchy
        enemyParent = new GameObject("Enemies");
        lootParent = new GameObject("Loot");
        shopParent = new GameObject("Shops");
        trapParent = new GameObject("Traps");
    }

    public void GenerateRooms(int maxRooms, int minSize, int maxSize)
    {
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(minSize, maxSize + 1);
            int roomHeight = Random.Range(minSize, maxSize + 1);
            int x = Random.Range(1, gridWidth - roomWidth - 1);
            int y = Random.Range(1, gridHeight - roomHeight - 1);

            Room newRoom = new Room(x, y, roomWidth, roomHeight);

            // Convert grid coordinates to world coordinates
            int worldStartX = x - gridWidth / 2;
            int worldEndX = worldStartX + roomWidth;
            int worldStartY = y - gridHeight / 2;
            int worldEndY = worldStartY + roomHeight;

            // Check if the room overlaps with the spawn room (-10, -10) to (10, 10)
            if (worldStartX < 13 && worldEndX > -13 && worldStartY < 13 && worldEndY > -13)
            {
                Debug.Log($"Skipped room at ({worldStartX}, {worldStartY}) because it overlaps the spawn room.");
                continue; // Skip this room
            }

            // Check if the room overlaps any existing room with a buffer of 7 tiles
            if (!Rooms.Exists(r =>
                newRoom.x < r.x + r.width + 7 &&
                newRoom.x + newRoom.width + 7 > r.x &&
                newRoom.y < r.y + r.height + 7 &&
                newRoom.y + newRoom.height + 7 > r.y))
            {
                Rooms.Add(newRoom);
                for (int rx = x; rx < x + roomWidth; rx++)
                {
                    for (int ry = y; ry < y + roomHeight; ry++)
                    {
                        grid[rx, ry] = 1; // Mark as floor
                    }
                }

                // Generate enemies, loot, and shops based on spawn chances
                SpawnObjectsInRoom(newRoom, x, y, roomWidth, roomHeight);
            }
        }

        // Ensure minimum counts are met
        EnsureMinimumCounts();

        // Debug the totals after all rooms are generated
        Debug.Log($"Total Enemies Spawned: {enemyCount}");
        Debug.Log($"Total Loot Spawned: {lootCount}");
        Debug.Log($"Total Shops Spawned: {shopCount}");
        Debug.Log($"Total Traps Spawned: {trapCount}");
    }

    private void EnsureMinimumCounts()
    {
        System.Random random = new System.Random();

        // Ensure at least 3 enemies
        while (enemyCount < 3)
        {
            // Pick a random enemy prefab from the list
            GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // Spawn the selected enemy
            SpawnInRandomRoom(selectedEnemyPrefab, enemyParent); // Call SpawnRandomEnemy with dummy values for coordinates
            enemyCount++;
        }

        // Ensure at least 2 loot
        while (lootCount < 2)
        {
            SpawnInRandomRoom(lootPrefab, lootParent);
            lootCount++;
        }

        // Ensure at least 1 shop
        while (shopCount < 1)
        {
            SpawnInRandomRoom(shopPrefab, shopParent);
            shopCount++;
        }

        // Ensure at least 10 traps
        while (trapCount < 10)
        {
            SpawnInRandomRoom(trapPrefab, trapParent);
            trapCount++;
        }
    }

    private void SpawnInRandomRoom(GameObject prefab, GameObject parent)
    {
        if (Rooms.Count == 0) return;

        Room randomRoom = Rooms[Random.Range(0, Rooms.Count)];
        SpawnPrefab(prefab, randomRoom.x, randomRoom.y, randomRoom.width, randomRoom.height, parent);
    }

    private void SpawnObjectsInRoom(Room room, int roomStartX, int roomStartY, int roomWidth, int roomHeight)
    {
        // 0.01 chance to spawn 20 enemies
        if (Random.Range(0f, 1f) < 0.001f)
        {
            // Spawn 20 enemies
            for (int i = 0; i < 20; i++)
            {
                GameObject enemy = SpawnRandomEnemy(roomStartX, roomStartY, roomWidth, roomHeight);
                enemyCount++;
                spawnedObjects.Add(enemy);
            }
        }
        // 40% chance for a single enemy if the 0.01 chance didn't trigger
        else if (Random.Range(0f, 1f) < 0.4f)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject enemy = SpawnRandomEnemy(roomStartX, roomStartY, roomWidth, roomHeight);
                enemyCount++;
                spawnedObjects.Add(enemy);
            }

        }

        // 40% chance for loot
        else if (Random.Range(0f, 1f) < 0.4f)
        {
            GameObject loot = SpawnPrefab(lootPrefab, roomStartX, roomStartY, roomWidth, roomHeight, lootParent);
            lootCount++;
            spawnedObjects.Add(loot);

            if (Random.Range(0f, 1f) < 0.2f)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject enemy = SpawnRandomEnemy(roomStartX, roomStartY, roomWidth, roomHeight);
                    enemyCount++;
                    spawnedObjects.Add(enemy);
                }
            }
        }

        // 20% chance for shop
        else if (Random.Range(0f, 1f) < 0.2f)
        {
            GameObject shop = SpawnPrefab(shopPrefab, roomStartX, roomStartY, roomWidth, roomHeight, shopParent);
            shopCount++;
            spawnedObjects.Add(shop);
        }
    }

    private GameObject SpawnRandomEnemy(int roomStartX, int roomStartY, int roomWidth, int roomHeight)
    {
        // Pick a random enemy prefab from the list
        GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // Spawn the selected enemy
        return SpawnPrefab(selectedEnemyPrefab, roomStartX, roomStartY, roomWidth, roomHeight, enemyParent);
    }

    private GameObject SpawnPrefab(GameObject prefab, int roomStartX, int roomStartY, int roomWidth, int roomHeight, GameObject parent)
    {
        bool spawned = false;
        GameObject spawnedObject = null;

        while (!spawned)
        {
            int spawnX = Random.Range(roomStartX + 1, roomStartX + roomWidth - 1);
            int spawnY = Random.Range(roomStartY + 1, roomStartY + roomHeight - 1);

            if (grid[spawnX, spawnY] == 1)
            {
                bool validSpawn = true;

                // Check a 3x3 area around the spawn location
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int checkX = spawnX + offsetX;
                        int checkY = spawnY + offsetY;

                        // Ensure the check stays within the bounds of the room
                        if (checkX >= roomStartX && checkX < roomStartX + roomWidth &&
                            checkY >= roomStartY && checkY < roomStartY + roomHeight)
                        {
                            if (grid[checkX, checkY] != 1)
                            {
                                validSpawn = false;
                                break;
                            }
                        }
                        else
                        {
                            validSpawn = false;
                            break;
                        }
                    }

                    if (!validSpawn) break;
                }

                // If the spawn location is valid, instantiate the prefab
                if (validSpawn)
                {
                    float worldPosX = spawnX - gridWidth / 2f;
                    float worldPosY = spawnY - gridHeight / 2f;

                    spawnedObject = GameObject.Instantiate(prefab, new Vector3(worldPosX, worldPosY, -10), Quaternion.identity);
                    spawnedObject.transform.parent = parent.transform; // Assign to parent
                    spawned = true;
                }
            }
        }

        return spawnedObject;
    }

}
