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

            Vector2Int currentCenter = GetRoomCenter(current);
            Vector2Int previousCenter = GetRoomCenter(previous);

            // Create horizontal hallway first
            CreateHorizontalHallway(previousCenter.x, currentCenter.x, previousCenter.y);

            // Add corner where the horizontal and vertical hallways meet
            CreateCorner(currentCenter.x, previousCenter.y);

            // Create vertical hallway second
            CreateVerticalHallway(previousCenter.y, currentCenter.y, currentCenter.x);
        }
    }

    private Vector2Int GetRoomCenter(Room room)
    {
        return new Vector2Int(
            room.x + room.width / 2,
            room.y + room.height / 2
        );
    }

    private void CreateHorizontalHallway(int startX, int endX, int fixedY)
    {
        for (int x = Mathf.Min(startX, endX); x <= Mathf.Max(startX, endX); x++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int yPos = fixedY + dy;
                if (IsWithinBounds(x, yPos))
                {
                    grid[x, yPos] = 1; // Mark as hallway
                }
            }
        }
    }

    private void CreateVerticalHallway(int startY, int endY, int fixedX)
    {
        for (int y = Mathf.Min(startY, endY); y <= Mathf.Max(startY, endY); y++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int xPos = fixedX + dx;
                if (IsWithinBounds(xPos, y))
                {
                    grid[xPos, y] = 1; // Mark as hallway
                }
            }
        }
    }

    private void CreateCorner(int cornerX, int cornerY)
    {
        // Create a 3x3 square for the corner at (cornerX, cornerY)
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int xPos = cornerX + dx;
                int yPos = cornerY + dy;

                if (IsWithinBounds(xPos, yPos))
                {
                    grid[xPos, yPos] = 1; // Mark as hallway
                }
            }
        }
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
    }
}
