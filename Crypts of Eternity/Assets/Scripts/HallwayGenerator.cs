using System.Collections.Generic; 
using UnityEngine;

public class HallwayGenerator
{
    private int[,] grid;
    private Room mainRoom;

    public HallwayGenerator(int[,] grid, Room mainRoom)
    {
        this.grid = grid;
        this.mainRoom = mainRoom;
    }

    public void ConnectRooms(List<Room> rooms)
    {
    // Get the center of the main room (spawn room)
    Vector2Int mainRoomCenter = GetRoomCenter(mainRoom);

    // Start by generating hallways from the main room to all other rooms
    foreach (Room room in rooms)
    {
        if (room == mainRoom) continue; // Skip the main room

        Vector2Int roomCenter = GetRoomCenter(room);

        // Debug: Log room and center positions
        Debug.Log($"Connecting room at {room.x}, {room.y} to spawn room at {mainRoom.x}, {mainRoom.y}");

        // Create a hallway from the main room to this room
        CreateHallway(mainRoomCenter, roomCenter);
    }
    }

    private Vector2Int GetRoomCenter(Room room)
    {
    // Room center calculation: Start point + half of width/height
    return new Vector2Int(room.x + room.width / 2, room.y + room.height / 2);
    }

    private void CreateHallway(Vector2Int start, Vector2Int end)
    {
    // Randomize whether to go horizontally or vertically first
    bool goHorizontalFirst = Random.Range(0, 2) == 0;

    if (goHorizontalFirst)
    {
        // Horizontal first
        CreateHorizontalHallway(start.x, end.x, start.y);
        CreateCorner(end.x, start.y);
        CreateVerticalHallway(start.y, end.y, end.x);
    }
    else
    {
        // Vertical first
        CreateVerticalHallway(start.y, end.y, start.x);
        CreateCorner(start.x, end.y);
        CreateHorizontalHallway(start.x, end.x, end.y);
    }
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
