using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile floorTile;
    [SerializeField] private Vector2Int roomSize = new Vector2Int(12, 8);
    [SerializeField] private string seedString = "";
    

    private System.Random random;

    public void GenerateLevel(string seed = "")
    {
        // Use provided seed or generate random one
        if (string.IsNullOrEmpty(seed))
        {
            seed = DateTime.Now.Ticks.ToString();
        }
        seedString = seed;
        random = new System.Random(seed.GetHashCode());

        ClearLevel();
        GenerateFloor();
        GenerateWalls();
        // Future expansion:
        // GenerateEnemies();
        // GenerateItems();
        // etc.
    }

    private void ClearLevel()
    {
        wallTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();
    }

    private void GenerateFloor()
    {
        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
        }
    }

    /// <summary>
    /// Generates the walls for the level. Inner walls are generated randomly, the amount of walls is based on the room size.
    /// </summary>
    private void GenerateWalls()
    {
        // Generate outer walls
        for (int x = -1; x <= roomSize.x; x++)
        {
            for (int y = -1; y <= roomSize.y; y++)
            {
                if (x == -1 || x == roomSize.x || y == -1 || y == roomSize.y)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }

        float roomArea = roomSize.x * roomSize.y;
        int minWalls = Mathf.FloorToInt(roomArea * 0.1f);
        int maxWalls = Mathf.FloorToInt(roomArea * 0.2f);

        int numInnerWalls = random.Next(minWalls, maxWalls + 1);

        // Generate some random inner walls
        for (int i = 0; i < numInnerWalls; i++)
        {
            int x = random.Next(0, roomSize.x);
            int y = random.Next(0, roomSize.y);
            wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
        }
    }

    public string GetCurrentSeed()
    {
        return seedString;
    }
}
