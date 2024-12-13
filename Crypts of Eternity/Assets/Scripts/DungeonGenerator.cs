using UnityEngine;

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
    private BossRoomGenerator bossRoomGenerator;

    // Expose grid-related data
    public int[,] Grid => grid;
    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;

    // Public Prefabs for Enemy, Loot, and Shop
    public GameObject enemyPrefab;
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
            enemyPrefab = enemyPrefab,
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

        // Generate the boss room
        GameObject bossRoomObject = new GameObject("BossRoomGenerator");
        bossRoomGenerator = bossRoomObject.AddComponent<BossRoomGenerator>();
        bossRoomGenerator.Initialize(grid, gridWidth, gridHeight); // Add an Initialize method to BossRoomGenerator

        PlaceTeleportDoor();
        Debug.Log("Boss Room Generated and Linked.");

        // Generate the rooms
        roomGenerator.GenerateRooms(maxRooms, minRoomSize, maxRoomSize);
        Debug.Log($"Rooms Generated: {roomGenerator.Rooms.Count}");

        // Create HallwayGenerator with the mainRoom
        hallwayGenerator = new HallwayGenerator(grid, mainRoom);

        Debug.Log("Connecting Rooms with Hallways...");
        hallwayGenerator.ConnectRooms(roomGenerator.Rooms);
        Debug.Log("Hallways Connected");

        Debug.Log("Adding Walls...");
        AddWalls();
        Debug.Log("Walls Added");

        
        // Pass the teleport door prefab to the boss room generator
        bossRoomGenerator.teleportDoorPrefab = teleportDoorPrefab;

        int bossRoomStartX = 400; // Fixed location for boss room
        int bossRoomStartY = 400;
        int bossRoomWidth = 20;
        int bossRoomHeight = 20;

        bossRoomGenerator.GenerateBossRoom(bossRoomStartX, bossRoomStartY, bossRoomWidth, bossRoomHeight);

        
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

    public void PlaceTeleportDoor()
    {
        // Set the fixed position for the door (world coordinates)
        Vector3 doorWorldPosition = new Vector3(10, 0, -10); // Fixed world position for the door

        // Ensure the door prefab is placed correctly without affecting the grid tile
        Instantiate(teleportDoorPrefab, doorWorldPosition, Quaternion.identity);

        Debug.Log("Teleport Door placed at " + doorWorldPosition);
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

    void DebugGrid()
    {
        Debug.Log("Grid Layout:");
        string gridString = "";
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                gridString += grid[x, y] + " ";
            }
            gridString += "\n";
        }
        Debug.Log(gridString);
    }
}
