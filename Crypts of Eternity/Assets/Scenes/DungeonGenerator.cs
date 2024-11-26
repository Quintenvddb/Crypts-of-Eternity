using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth = 400;
    public int gridHeight = 400;

    public int maxRooms = 100;
    public int minRoomSize = 20;
    public int maxRoomSize = 50;

    public Tilemap tilemap; // The Tilemap to paint on.
    public TileBase[] floorTiles; // Array to hold floor tiles.
    public TileBase[] wallTiles; // Array to hold wall tiles.

    private int[,] grid;
    private RoomGenerator roomGenerator;
    private HallwayGenerator hallwayGenerator;

    void Start()
    {
        grid = new int[gridWidth, gridHeight];
        roomGenerator = new RoomGenerator(grid, gridWidth, gridHeight);
        hallwayGenerator = new HallwayGenerator(grid);

        GenerateDungeon();
        RenderDungeon();
    }

    void GenerateDungeon()
    {
        roomGenerator.GenerateRooms(maxRooms, minRoomSize, maxRoomSize);
        hallwayGenerator.ConnectRooms(roomGenerator.Rooms);
        AddWalls();
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
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0);

                if (grid[x, y] == 1) // Floor
                {
                    tilemap.SetTile(tilePosition, GetRandomTile(floorTiles));
                }
                else if (grid[x, y] == 2) // Wall
                {
                    tilemap.SetTile(tilePosition, GetRandomTile(wallTiles));
                }
            }
        }
    }

    TileBase GetRandomTile(TileBase[] tiles)
    {
        if (tiles.Length == 0) return null;
        return tiles[Random.Range(0, tiles.Length)];
    }
}
