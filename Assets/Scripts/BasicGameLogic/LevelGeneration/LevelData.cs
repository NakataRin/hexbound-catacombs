using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    public string seed;
    public Vector2Int roomSize;
    public Vector3Int[] wallPositions;
    // Future expansion:
    // public EnemyData[] enemies;
    // public ItemData[] items;
    // etc.

    public LevelData(string seed, Vector2Int roomSize, Vector3Int[] wallPositions)
    {
        this.seed = seed;
        this.roomSize = roomSize;
        this.wallPositions = wallPositions;
    }
}

// Future expansion example:
[Serializable]
public class EnemyData
{
    public Vector3Int position;
    public string enemyType;
}
