using UnityEngine;

public class BossRoomGenerator : MonoBehaviour
{
    private int[,] grid;
    private int gridWidth, gridHeight;
    public GameObject teleportDoorPrefab;


    public void Initialize(int[,] grid, int width, int height)
    {
        this.grid = grid;
        this.gridWidth = width;
        this.gridHeight = height;
    }

    public void GenerateBossRoom(int startX, int startY, int roomWidth, int roomHeight)
    {
        Debug.Log("Generating Boss Room...");

        int floorStartX = startX + (roomWidth - 16) / 2;
        int floorStartY = startY + (roomHeight - 16) / 2;

        // Create the outer walls of the boss room
        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    grid[x, y] = 2; // Mark as wall tile
                }
            }
        }

        // Create the inner floor of the boss room
        for (int x = floorStartX; x < floorStartX + 16; x++)
        {
            for (int y = floorStartY; y < floorStartY + 16; y++)
            {
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    grid[x, y] = 1; // Mark as floor tile
                }
            }
        }

        // Fill remaining tiles with background
        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight && grid[x, y] == 0)
                {
                    grid[x, y] = 3; // Mark as background tile
                }
            }
        }

        Debug.Log("Boss Room Generated at (" + startX + ", " + startY + ").");
    }

}
