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
    public GameObject enemyPrefab;
    public GameObject lootPrefab;
    public GameObject shopPrefab;

    // Counters for spawned objects
    private int enemyCount = 0;
    private int lootCount = 0;
    private int shopCount = 0;

    public RoomGenerator(int[,] grid, int width, int height)
    {
        this.grid = grid;
        this.gridWidth = width;
        this.gridHeight = height;
        Rooms = new List<Room>();
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
            if (worldStartX < 11 && worldEndX > -11 && worldStartY < 11 && worldEndY > -11)
            {
                Debug.Log($"Skipped room at ({worldStartX}, {worldStartY}) because it overlaps the spawn room.");
                continue; // Skip this room
            }

            // Check if the room overlaps any existing room
            if (!Rooms.Exists(r => r.Overlaps(newRoom)))
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

        // Debug the totals after all rooms are generated
        Debug.Log($"Total Enemies Spawned: {enemyCount}");
        Debug.Log($"Total Loot Spawned: {lootCount}");
        Debug.Log($"Total Shops Spawned: {shopCount}");
    }

    private void SpawnObjectsInRoom(Room room, int roomStartX, int roomStartY, int roomWidth, int roomHeight)
    {
        // 40% chance for enemy
        if (Random.Range(0f, 1f) < 0.4f)
        {
            GameObject enemy = SpawnPrefab(enemyPrefab, roomStartX, roomStartY, roomWidth, roomHeight);
            enemyCount++; // Increment enemy count
            spawnedObjects.Add(enemy); // Track spawned object
        }

        // 40% chance for loot
        if (Random.Range(0f, 1f) < 0.4f)
        {
            GameObject loot = SpawnPrefab(lootPrefab, roomStartX, roomStartY, roomWidth, roomHeight);
            lootCount++; // Increment loot count
            spawnedObjects.Add(loot); // Track spawned object
        }

        // 20% chance for shop
        if (Random.Range(0f, 1f) < 0.2f)
        {
            GameObject shop = SpawnPrefab(shopPrefab, roomStartX, roomStartY, roomWidth, roomHeight);
            shopCount++; // Increment shop count
            spawnedObjects.Add(shop); // Track spawned object
        }
    }

    private GameObject SpawnPrefab(GameObject prefab, int roomStartX, int roomStartY, int roomWidth, int roomHeight)
    {
        // Try to find a valid floor position for the spawn
        bool spawned = false;
        GameObject spawnedObject = null;
        while (!spawned)
        {
            // Random position inside the room (aligned with grid)
            int spawnX = Random.Range(roomStartX, roomStartX + roomWidth);
            int spawnY = Random.Range(roomStartY, roomStartY + roomHeight);

            // Ensure it's a valid floor tile (grid value is 1)
            if (grid[spawnX, spawnY] == 1)
            {
                // Convert grid coordinates to world space
                float worldPosX = spawnX - gridWidth / 2f;  // Correct for negative grid position
                float worldPosY = spawnY - gridHeight / 2f; // Correct for negative grid position

                // Spawn the prefab at the world position
                spawnedObject = GameObject.Instantiate(prefab, new Vector3(worldPosX, worldPosY, -10), Quaternion.identity);
                spawned = true; // Successfully spawned the object
            }
        }
        return spawnedObject;
    }
}
