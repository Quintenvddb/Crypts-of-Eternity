using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth = 100;
    public int gridHeight = 100;

    public int maxRooms = 20;
    public int minRoomSize = 7;
    public int maxRoomSize = 15;

    private int[,] grid;
    private RoomGenerator roomGenerator;
    private HallwayGenerator hallwayGenerator;

    // Expose grid-related data
    public int[,] Grid => grid;
    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;

    // Public Prefabs for Enemy, Loot, and Shop
    public GameObject enemyPrefab;
    public GameObject lootPrefab;
    public GameObject shopPrefab;

    void Start()
    {
        Debug.Log("Starting Dungeon Generation...");

        grid = new int[gridWidth, gridHeight];
        roomGenerator = new RoomGenerator(grid, gridWidth, gridHeight)
        {
            enemyPrefab = enemyPrefab,
            lootPrefab = lootPrefab,
            shopPrefab = shopPrefab
        };
        hallwayGenerator = new HallwayGenerator(grid);

        GenerateDungeon();
        DebugGrid();
    }

    void GenerateDungeon()
    {
        Debug.Log("Generating Initial Spawn Room...");
        GenerateSpawnRoom(-10, -10, 10, 10); // Define the spawn room area
        Debug.Log("Spawn Room Created.");

        Debug.Log("Generating Rooms...");
        roomGenerator.GenerateRooms(maxRooms, minRoomSize, maxRoomSize);
        Debug.Log($"Rooms Generated: {roomGenerator.Rooms.Count}");

        Debug.Log("Connecting Rooms with Hallways...");
        hallwayGenerator.ConnectRooms(roomGenerator.Rooms);
        Debug.Log("Hallways Connected");

        Debug.Log("Adding Walls...");
        AddWalls();
        Debug.Log("Walls Added");
    }

    void GenerateSpawnRoom(int startX, int startY, int endX, int endY)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                int gridX = x + gridWidth / 2;
                int gridY = y + gridHeight / 2;

                if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
                {
                    grid[gridX, gridY] = 1; // Mark as floor tile
                }
            }
        }
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
