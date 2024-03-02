using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector2Int[] cells { get; private set; }
    public Vector2Int position { get; private set; }



    // might need to vector 3 Int
    public void Initialize(Board board, Vector2Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if (cells == null)
        {
            cells = new Vector2Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            cells[i] = data.cells[i];
        }
    }
}
