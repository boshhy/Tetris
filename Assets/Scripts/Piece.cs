
//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector2Int[] cells { get; private set; }
    public Vector2Int position { get; private set; }
    public int currentRotationIndex { get; private set; }



    // might need to vector 3 Int
    public void Initialize(Board board, Vector2Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        currentRotationIndex = 0;

        if (cells == null)
        {
            cells = new Vector2Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            cells[i] = data.cells[i];
        }
    }

    public void Update()
    {
        board.Clear(this);

        if (Input.GetKeyDown(KeyCode.K))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLocation(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLocation(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveLocation(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            HardDrop();
        }

        board.Set(this);
    }

    public void HardDrop()
    {
        while (MoveLocation(Vector2Int.down)) { continue; }
    }

    public bool MoveLocation(Vector2Int maneuver)
    {
        Vector2Int newPosition = position;
        newPosition.x += maneuver.x;
        newPosition.y += maneuver.y;

        bool isValid = board.IsValidPosition(this, newPosition);
        if (isValid)
        {
            position = newPosition;
        }

        return isValid;
    }

    private void Rotate(int rotation)
    {
        currentRotationIndex = Wrap(currentRotationIndex + rotation, 0, 4);

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2 cell = cells[i];
            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * rotation) + (cell.y * Data.RotationMatrix[1] * rotation));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * rotation) + (cell.y * Data.RotationMatrix[3] * rotation));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * rotation) + (cell.y * Data.RotationMatrix[1] * rotation));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * rotation) + (cell.y * Data.RotationMatrix[3] * rotation));
                    break;
            }

            cells[i] = new Vector2Int(x, y);
        }
    }

    private int Wrap(int input, int minInclusive, int maxExclusive)
    {
        if (input < minInclusive)
        {
            return maxExclusive - (minInclusive - input) % (maxExclusive - minInclusive);
        }
        else
        {
            return minInclusive + (input - minInclusive) % (maxExclusive - minInclusive);
        }
    }
}
