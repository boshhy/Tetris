using System;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Numerics;

public enum Tetromino
{
    I, J, L, O, S, T, Z
}

[Serializable]
public struct TetrominoData
{
    public Tile tile;
    public Tetromino tetromino;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        cells = Data.Cells[tetromino];
        wallKicks = Data.WallKicks[tetromino];
    }
}