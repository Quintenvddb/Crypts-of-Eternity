using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRenderer : MonoBehaviour
{
    public DungeonGenerator generator;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public TileBase[] floorTiles;
    public TileBase[] wallTiles;

    private bool[,] renderedTiles;

    void Start()
    {
        if (generator == null)
        {
            Debug.LogError("DungeonGenerator is not assigned!");
        }
    }

    void Update()
    {
        RenderDungeon();
    }

    void RenderDungeon()
    {
        if (generator == null)
        {
            Debug.LogError("DungeonGenerator is not assigned!");
            return;
        }

        int[,] grid = generator.Grid;
        int gridWidth = generator.GridWidth;
        int gridHeight = generator.GridHeight;

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        Vector3 cameraPos = mainCamera.transform.position;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.aspect * halfHeight;

        int minX = Mathf.FloorToInt(cameraPos.x - halfWidth) - 2;
        int maxX = Mathf.CeilToInt(cameraPos.x + halfWidth) + 2;
        int minY = Mathf.FloorToInt(cameraPos.y - halfHeight) - 2;
        int maxY = Mathf.CeilToInt(cameraPos.y + halfHeight) + 2;

        minX = Mathf.Clamp(minX, -gridWidth / 2, gridWidth / 2 - 1);
        maxX = Mathf.Clamp(maxX, -gridWidth / 2, gridWidth / 2 - 1);
        minY = Mathf.Clamp(minY, -gridHeight / 2, gridHeight / 2 - 1);
        maxY = Mathf.Clamp(maxY, -gridHeight / 2, gridHeight / 2 - 1);

        if (renderedTiles == null)
        {
            renderedTiles = new bool[gridWidth, gridHeight];
        }

        // Render Tiles (Floor and Walls)
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                int gridX = x + gridWidth / 2;
                int gridY = y + gridHeight / 2;

                if (gridX >= 0 && gridX < gridWidth && gridY >= 0 && gridY < gridHeight)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);

                    if (!renderedTiles[gridX, gridY])
                    {
                        if (grid[gridX, gridY] == 1) // Floor
                        {
                            floorTilemap.SetTile(tilePosition, GetRandomTile(floorTiles));
                        }
                        else if (grid[gridX, gridY] == 2) // Wall
                        {
                            wallTilemap.SetTile(tilePosition, GetRandomTile(wallTiles, true));
                        }

                        renderedTiles[gridX, gridY] = true;
                    }
                }
            }
        }

        // Render Enemies, Loot, and Shops (based on visibility)
        foreach (var obj in generator.roomGenerator.SpawnedObjects)
        {
            // Check if the object is within the camera's view
            Vector3 screenPos = mainCamera.WorldToScreenPoint(obj.transform.position);

            if (screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height)
            {
                // Only render the object if it's within the screen bounds
                obj.SetActive(true); // Or other logic to enable rendering
            }
            else
            {
                obj.SetActive(false); // Hide the object when it's outside the view
            }
        }
    }

    TileBase GetRandomTile(TileBase[] tiles, bool isWall = false)
    {
        if (tiles.Length == 0)
        {
            Debug.LogWarning("Tile array is empty!");
            return null;
        }

        TileBase selectedTile = tiles[Random.Range(0, tiles.Length)];
        return selectedTile;
    }
}
