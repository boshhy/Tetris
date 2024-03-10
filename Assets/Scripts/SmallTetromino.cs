
using System;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Numerics;

public enum SmallTetromino
{
    I, J, L, O, S, T, Z
}

[Serializable]
public struct SmallTetrominoData
{
    public Tile tile;
    public SmallTetromino tetromino;
    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        cells = SmallData.Cells[tetromino];
    }
}