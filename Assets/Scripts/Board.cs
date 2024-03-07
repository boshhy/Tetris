using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    public Piece activePiece { get; private set; }
    public Vector2Int spawnPosition = new Vector2Int(-1, 8);
    public Vector2Int validBoardSize = new Vector2Int(10, 20);

    public RectInt validBounds
    {
        get
        {
            Vector2Int leftBottomCornerLocation = new Vector2Int(-validBoardSize.x / 2, -validBoardSize.y / 2);
            return new RectInt(leftBottomCornerLocation, validBoardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponent<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);
        Set(activePiece);

    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile((Vector3Int)tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile((Vector3Int)tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector2Int possiblePosition)
    {
        RectInt bounds = validBounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int possibleTilePostion = piece.cells[i] + possiblePosition;

            if (!bounds.Contains(possibleTilePostion))
            {
                return false;
            }
            if (tilemap.HasTile((Vector3Int)possibleTilePostion))
            {
                return false;
            }
        }
        return true;
    }
}
