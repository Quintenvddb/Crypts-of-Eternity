using UnityEngine;

public class Room
{
    public int x, y; // Top-left corner of the room
    public int width, height;

    public enum RoomType { Empty, Enemy, Loot, Shop }
    public RoomType Type { get; private set; }

    public Room(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        Type = RoomType.Empty; // Default type
    }

    public void AssignRoomType(bool isSpawnRoom)
    {
        if (isSpawnRoom)
        {
            Type = RoomType.Empty;
            return;
        }

        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.4f)
        {
            Type = RoomType.Enemy;
        }
        else if (randomValue < 0.8f)
        {
            Type = RoomType.Loot;
        }
        else
        {
            Type = RoomType.Shop;
        }
    }

    public bool Overlaps(Room other)
    {
        return x < other.x + other.width &&
               x + width > other.x &&
               y < other.y + other.height &&
               y + height > other.y;
    }
}
