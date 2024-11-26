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

            for (int x = Mathf.Min(currentCenter.x, previousCenter.x); x <= Mathf.Max(currentCenter.x, previousCenter.x); x++)
            {
                grid[x, previousCenter.y] = 1;
            }
            for (int y = Mathf.Min(currentCenter.y, previousCenter.y); y <= Mathf.Max(currentCenter.y, previousCenter.y); y++)
            {
                grid[currentCenter.x, y] = 1;
            }
        }
    }
}
