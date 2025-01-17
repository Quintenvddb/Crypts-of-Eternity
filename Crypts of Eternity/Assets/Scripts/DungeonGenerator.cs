using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth = 100;
    public int gridHeight = 100;

    public int maxRooms = 20;
    public int minRoomSize = 7;
    public int maxRoomSize = 15;

    private int[,] grid;
    public RoomGenerator roomGenerator;
    private HallwayGenerator hallwayGenerator;

    // Expose grid-related data
    public int[,] Grid => grid;
    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;

    // Public Prefabs for Enemy, Loot, and Shop
    public List<GameObject> enemyPrefabs;
    public GameObject lootPrefab;
    public GameObject shopPrefab;
    public GameObject teleportDoorPrefab;

    private Room mainRoom;

    void Awake()
    {
        Debug.Log("Initializing Dungeon Generator...");

        grid = new int[gridWidth, gridHeight];
        roomGenerator = new RoomGenerator(grid, gridWidth, gridHeight)
        {
            enemyPrefabs = enemyPrefabs,
            lootPrefab = lootPrefab,
            shopPrefab = shopPrefab
        };
    }

    void Start()
    {
        Debug.Log("Starting Dungeon Generation...");

        // Generate the spawn room and set it as the main room
        GenerateSpawnRoom(-10, -10, 10, 10);
        Debug.Log("Spawn Room Created.");

        // Generate the dungeon rooms
        roomGenerator.GenerateRooms(maxRooms, minRoomSize, maxRoomSize);
        Debug.Log($"Rooms Generated: {roomGenerator.Rooms.Count}");
        
        // Create HallwayGenerator with the mainRoom
        hallwayGenerator = new HallwayGenerator(grid, mainRoom);

        Debug.Log("Connecting Rooms with Hallways...");
        hallwayGenerator.ConnectRooms(roomGenerator.Rooms);
        Debug.Log("Hallways Connected");

        // Expand the grid and add the boss room
        GenerateBossRoom(78, -10, 98, 10); // Place at (100, 0) relative to the grid's center
        Debug.Log("Boss Room Created at (100, 0).");


        Debug.Log("Adding Walls...");
        AddWalls();
        Debug.Log("Walls Added");
    }

    void GenerateSpawnRoom(int startX, int startY, int endX, int endY)
    {
        int gridCenterX = gridWidth / 2;
        int gridCenterY = gridHeight / 2;

        // Create the spawn room in the center of the grid
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                int gridX = x + gridCenterX; // Offset by center
                int gridY = y + gridCenterY;

                if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
                {
                    grid[gridX, gridY] = 1; // Mark as floor tile
                }
            }
        }

        // Define the spawn room (main room)
        mainRoom = new Room(gridCenterX + startX, gridCenterY + startY, endX - startX + 1, endY - startY + 1);
    }

    void GenerateBossRoom(int startX, int startY, int endX, int endY)
{
    // Expand the grid to ensure enough space for the boss room
    ExpandGrid(60, 0);

    int gridCenterX = gridWidth / 2;
    int gridCenterY = gridHeight / 2;

    // Generate the boss room at a fixed position relative to the dungeon
    for (int x = startX; x <= endX; x++)
    {
        for (int y = startY; y <= endY; y++)
        {
            int gridX = x + gridCenterX; // Offset by center
            int gridY = y + gridCenterY;

            if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
            {
                grid[gridX, gridY] = 1; // Mark as floor tile
            }
            else
            {
                Debug.LogWarning($"Boss room tile ({gridX}, {gridY}) is outside the grid bounds.");
            }
        }
    }

    // Place the teleport door leading to the boss room
    PlaceTeleportDoor(new Vector3(10, 8, -10)); // Adjust as needed
}

void PlaceTeleportDoor(Vector3 doorWorldPosition)
{
    // Ensure the door prefab is instantiated at the correct position
    GameObject door = Instantiate(teleportDoorPrefab, doorWorldPosition, Quaternion.identity);

    // Set the target position for the teleport to the center of the boss room
    TeleportDoor teleportDoor = door.GetComponent<TeleportDoor>();
    if (teleportDoor != null)
    {
        teleportDoor.SetTargetPosition(new Vector3(80, 0, -10));
        Debug.Log("Teleport Door target set for Boss Room.");
    }
}

    void ExpandGrid(int expandWidth, int expandHeight)
    {
        Debug.Log($"Expanding grid by Width: {expandWidth}, Height: {expandHeight}");

        int newWidth = gridWidth + expandWidth;
        int newHeight = gridHeight + expandHeight;

        int[,] newGrid = new int[newWidth, newHeight];
        int offsetX = expandWidth / 2;
        int offsetY = expandHeight / 2;

        // Copy existing grid to the new grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                newGrid[x + offsetX, y + offsetY] = grid[x, y];
            }
        }

        // Update grid dimensions
        gridWidth = newWidth;
        gridHeight = newHeight;
        grid = newGrid;

        Debug.Log($"Grid expanded to Width: {gridWidth}, Height: {gridHeight}");
    }

    void AddWalls()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == 1) // Floor
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int nx = x + dx, ny = y + dy;
                            if (nx >= 0 && ny >= 0 && nx < gridWidth && ny < gridHeight && grid[nx, ny] == 0)
                            {
                                grid[nx, ny] = 2; // Mark as wall
                            }
                        }
                    }
                }
            }
        }
    }
}
