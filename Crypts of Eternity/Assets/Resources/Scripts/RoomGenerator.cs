using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int[,] grid;
    private int gridWidth, gridHeight;
    public List<Room> Rooms { get; private set; }

    public RoomGenerator(int[,] grid, int width, int height)
    {
        this.grid = grid;
        this.gridWidth = width;
        this.gridHeight = height;
        Rooms = new List<Room>();
    }

    public void GenerateRooms(int maxRooms, int minSize, int maxSize)
    {
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(minSize, maxSize + 1);
            int roomHeight = Random.Range(minSize, maxSize + 1);
            int x = Random.Range(1, gridWidth - roomWidth - 1);
            int y = Random.Range(1, gridHeight - roomHeight - 1);

            Room newRoom = new Room(x, y, roomWidth, roomHeight);

            // Convert grid coordinates to world coordinates for the spawn room check
            int worldStartX = x - gridWidth / 2;
            int worldEndX = worldStartX + roomWidth;
            int worldStartY = y - gridHeight / 2;
            int worldEndY = worldStartY + roomHeight;

            // Check if the room overlaps with the spawn room (-10, -10) to (10, 10)
            if (worldStartX < 11 && worldEndX > -11 && worldStartY < 11 && worldEndY > -11)
            {
                Debug.Log($"Skipped room at ({worldStartX}, {worldStartY}) because it overlaps the spawn room.");
                continue; // Skip this room
            }

            // Check if the room overlaps any existing room
            if (!Rooms.Exists(r => r.Overlaps(newRoom)))
            {
                Rooms.Add(newRoom);
                for (int rx = x; rx < x + roomWidth; rx++)
                {
                    for (int ry = y; ry < y + roomHeight; ry++)
                    {
                        grid[rx, ry] = 1; // Mark as floor
                    }
                }
            }
        }
    }
}
