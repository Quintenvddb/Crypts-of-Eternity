using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth = 100;
    public int gridHeight = 100;

    public int maxRooms = 20;
    public int minRoomSize = 6;
    public int maxRoomSize = 12;

    public Tilemap tilemap; // The Tilemap to paint on.
    public TileBase[] floorTiles; // Array to hold floor tiles.
    public TileBase[] wallTiles; // Array to hold wall tiles.

    private int[,] grid;
    private RoomGenerator roomGenerator;
    private HallwayGenerator hallwayGenerator;

    void Start()
    {
        Debug.Log("Starting Dungeon Generation...");
        
        grid = new int[gridWidth, gridHeight];
        roomGenerator = new RoomGenerator(grid, gridWidth, gridHeight);
        hallwayGenerator = new HallwayGenerator(grid);

        GenerateDungeon();
        DebugGrid(); // Display the grid in the console for debugging.
        RenderDungeon();
    }

    void GenerateDungeon()
    {
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
        Debug.Log("Rendering Dungeon...");
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0);

                if (grid[x, y] == 1) // Floor
                {
                    var tile = GetRandomTile(floorTiles);
                    tilemap.SetTile(tilePosition, tile);
                    Debug.Log($"Placed Floor at {tilePosition}");
                }
                else if (grid[x, y] == 2) // Wall
                {
                    var tile = GetRandomTile(wallTiles);
                    tilemap.SetTile(tilePosition, tile);
                    Debug.Log($"Placed Wall at {tilePosition}");
                }
            }
        }

        Debug.Log("Dungeon Rendered");
    }

    TileBase GetRandomTile(TileBase[] tiles)
    {
        if (tiles.Length == 0)
        {
            Debug.LogWarning("Tile array is empty!");
            return null;
        }
        var selectedTile = tiles[Random.Range(0, tiles.Length)];
        Debug.Log("Selected Tile: " + selectedTile.name);
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
