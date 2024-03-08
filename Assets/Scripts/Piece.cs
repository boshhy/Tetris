
//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector2Int[] cells { get; private set; }
    public Vector2Int position { get; private set; }
    public int currentRotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;



    // might need to vector 3 Int
    public void Initialize(Board board, Vector2Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        currentRotationIndex = 0;
        stepTime = Time.time + stepDelay;
        lockTime = 0f;


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

        lockTime += Time.deltaTime;

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
            AudioManager.instance.PlaySFX(6);
            MoveLocation(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            HardDrop();
        }

        if (Time.time >= stepTime)
        {
            Step();
        }

        board.Set(this);
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        MoveLocation(Vector2Int.down);

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        board.Set(this);
        AudioManager.instance.PlaySFX(10);
        board.ClearLines();
        board.SpawnPiece();
    }

    private void HardDrop()
    {
        AudioManager.instance.PlaySFX(0);
        while (MoveLocation(Vector2Int.down, true)) { continue; }
        Lock();

    }

    public bool MoveLocation(Vector2Int maneuver, bool isHardDrop = false)
    {
        Vector2Int newPosition = position;
        newPosition.x += maneuver.x;
        newPosition.y += maneuver.y;

        bool isValid = board.IsValidPosition(this, newPosition);
        if (isValid)
        {
            if (maneuver != Vector2Int.down)
            {
                AudioManager.instance.PlaySFX(3);
            }
            position = newPosition;
            lockTime = 0;
        }


        if (!isHardDrop && isValid)
        {
            if (!board.IsValidPosition(this, newPosition + Vector2Int.down))
            {
                AudioManager.instance.PlaySFX(2);
            }
        }

        return isValid;
    }

    private void ApplyRotationMatrix(int rotation)
    {

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

    private void Rotate(int rotation)
    {

        int originalRotationIndex = currentRotationIndex;
        currentRotationIndex = Wrap(currentRotationIndex + rotation, 0, 4);

        ApplyRotationMatrix(rotation);

        if (!TestWallKicks(originalRotationIndex, rotation))
        {
            currentRotationIndex = originalRotationIndex;
            ApplyRotationMatrix(-rotation);
        }
        else
        {
            AudioManager.instance.PlaySFX(5);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            if (MoveLocation(data.wallKicks[wallKickIndex, i]))
            {
                return true;
            }

        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }


        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
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
