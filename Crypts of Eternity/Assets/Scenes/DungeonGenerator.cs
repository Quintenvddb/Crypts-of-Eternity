using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth = 20;
    public int gridHeight = 20;

    public int maxRooms = 10;
    public int minRoomSize = 3;
    public int maxRoomSize = 5;

    public GameObject floorPrefab;
    public GameObject wallPrefab;

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
            // Adjust coordinates to match Unity's world space.
            Vector3 position = new Vector3(x - gridWidth/2, y - gridHeight/2, 0);  // x and y will now correspond to grid coordinates

            if (grid[x, y] == 1) // Floor
            {
                Instantiate(floorPrefab, position, Quaternion.identity);
            }
            else if (grid[x, y] == 2) // Wall
            {
                Instantiate(wallPrefab, position, Quaternion.identity);
            }
        }
    }
}
}
