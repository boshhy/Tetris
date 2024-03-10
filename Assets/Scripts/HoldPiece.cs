using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPiece : MonoBehaviour
{
    public Board board { get; private set; }
    public SmallTetrominoData data { get; private set; }
    public Vector2Int[] cells { get; private set; }
    public Vector2Int position { get; private set; }
    public int currentRotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;



    // might need to vector 3 Int
    public void Initialize(Board board, Vector2Int position, SmallTetrominoData smallData)
    {
        this.board = board;
        this.position = position;
        this.data = smallData;
        currentRotationIndex = 0;
        stepTime = Time.time + stepDelay;
        lockTime = 0f;


        if (cells == null)
        {
            cells = new Vector2Int[smallData.cells.Length];
        }

        for (int i = 0; i < smallData.cells.Length; i++)
        {
            cells[i] = smallData.cells[i];
        }
        Debug.Log("finished initalizeing holdpiece");
    }

    public void SwitchPiece(Piece piece, SmallTetromino[] smallTetrominos)
    {


    }
}
