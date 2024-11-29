using System.Collections.Generic;
using UnityEngine;

public class HallwayGenerator
{
    private int[,] grid;

    public HallwayGenerator(int[,] grid)
    {
        this.grid = grid;
    }

    public void ConnectRooms(List<Room> rooms)
{
    for (int i = 1; i < rooms.Count; i++)
    {
        Room current = rooms[i];
        Room previous = rooms[i - 1];

        Vector2Int currentCenter = new Vector2Int(
            current.x + current.width / 2,
            current.y + current.height / 2
        );
        Vector2Int previousCenter = new Vector2Int(
            previous.x + previous.width / 2,
            previous.y + previous.height / 2
        );

        // Create a horizontal hallway (3 tiles wide)
        for (int x = Mathf.Min(currentCenter.x, previousCenter.x); x <= Mathf.Max(currentCenter.x, previousCenter.x); x++)
        {
            // Create a 3-tile wide corridor at this x position
            for (int dy = -1; dy <= 1; dy++)
            {
                int yPos = previousCenter.y + dy;
                if (yPos >= 0 && yPos < grid.GetLength(1))  // Ensure the yPos is within bounds
                {
                    grid[x, yPos] = 1;
                }
            }
        }

        // Create a vertical hallway (3 tiles wide)
        for (int y = Mathf.Min(currentCenter.y, previousCenter.y); y <= Mathf.Max(currentCenter.y, previousCenter.y); y++)
        {
            // Create a 3-tile wide corridor at this y position
            for (int dx = -1; dx <= 1; dx++)
            {
                int xPos = currentCenter.x + dx;
                if (xPos >= 0 && xPos < grid.GetLength(0))  // Ensure the xPos is within bounds
                {
                    grid[xPos, y] = 1;
                }
            }
        }
    }
}
}