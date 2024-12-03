using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth = 100; // Total width of the grid.
    public int gridHeight = 100; // Total height of the grid.

    public int maxRooms = 20;
    public int minRoomSize = 7;
    public int maxRoomSize = 15;

    public Tilemap floorTilemap; // Tilemap for floor tiles.
    public Tilemap wallTilemap;  // Tilemap for wall tiles.
    public TileBase[] floorTiles; // Array to hold floor tiles.
    public TileBase[] wallTiles;  // Array to hold wall tiles.

    private int[,] grid;
    private RoomGenerator roomGenerator;
    private HallwayGenerator hallwayGenerator;

    private bool[,] renderedTiles; // Track rendered tiles persistently.

    void Start()
    {
        Debug.Log("Starting Dungeon Generation...");

        grid = new int[gridWidth, gridHeight]; // Positive-only grid.
        roomGenerator = new RoomGenerator(grid, gridWidth, gridHeight);
        hallwayGenerator = new HallwayGenerator(grid);

        GenerateDungeon();
        DebugGrid(); // Display the grid in the console for debugging.
    }

    void Update()
    {
        RenderDungeon(); // Render the dungeon every frame.
    }

    void GenerateDungeon()
    {
        Debug.Log("Generating Initial Spawn Room...");
        GenerateSpawnRoom(-10, -10, 10, 10); // Define the spawn room area.
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
                int gridX = x + gridWidth / 2; // Convert world coordinates to grid indices.
                int gridY = y + gridHeight / 2;

                if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
                {
                    grid[gridX, gridY] = 1; // Mark as floor tile.
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

    void RenderDungeon()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        Vector3 cameraPos = mainCamera.transform.position;

        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.aspect * halfHeight;

        int minX = Mathf.FloorToInt(cameraPos.x - halfWidth) - 2;
        int maxX = Mathf.CeilToInt(cameraPos.x + halfWidth) + 2;
        int minY = Mathf.FloorToInt(cameraPos.y - halfHeight) - 2;
        int maxY = Mathf.CeilToInt(cameraPos.y + halfHeight) + 2;

        minX = Mathf.Clamp(minX, -gridWidth / 2, gridWidth / 2 - 1);
        maxX = Mathf.Clamp(maxX, -gridWidth / 2, gridWidth / 2 - 1);
        minY = Mathf.Clamp(minY, -gridHeight / 2, gridHeight / 2 - 1);
        maxY = Mathf.Clamp(maxY, -gridHeight / 2, gridHeight / 2 - 1);

        if (renderedTiles == null)
        {
            renderedTiles = new bool[gridWidth, gridHeight];
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (renderedTiles[x, y])
                {
                    Vector3Int tilePosition = new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0);
                    if (!IsTileWithinBounds(tilePosition, minX, maxX, minY, maxY))
                    {
                        floorTilemap.SetTile(tilePosition, null); // Remove floor tile
                        wallTilemap.SetTile(tilePosition, null);  // Remove wall tile
                        renderedTiles[x, y] = false;
                    }
                }
            }
        }

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                int gridX = x + gridWidth / 2;
                int gridY = y + gridHeight / 2;

                if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);

                    if (!renderedTiles[gridX, gridY])
                    {
                        if (grid[gridX, gridY] == 1) // Floor
                        {
                            floorTilemap.SetTile(tilePosition, GetRandomTile(floorTiles));
                        }
                        else if (grid[gridX, gridY] == 2) // Wall
                        {
                            wallTilemap.SetTile(tilePosition, GetRandomTile(wallTiles, true));
                        }

                        renderedTiles[gridX, gridY] = true;
                    }
                }
            }
        }
    }

    bool IsTileWithinBounds(Vector3Int tilePosition, int minX, int maxX, int minY, int maxY)
    {
        int x = tilePosition.x;
        int y = tilePosition.y;

        return (x >= minX && x <= maxX && y >= minY && y <= maxY);
    }

    TileBase GetRandomTile(TileBase[] tiles, bool isWall = false)
    {
        if (tiles.Length == 0)
        {
            Debug.LogWarning("Tile array is empty!");
            return null;
        }

        TileBase selectedTile = tiles[Random.Range(0, tiles.Length)];

        if (isWall && selectedTile is Tile tile)
        {
            tile.colliderType = Tile.ColliderType.Sprite; // Enable collision for walls
        }

        return selectedTile;
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
