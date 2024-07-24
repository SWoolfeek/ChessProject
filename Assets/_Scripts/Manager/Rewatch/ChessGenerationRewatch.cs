using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessGenerationRewatch : ChessGenerationManager
{
    public static ChessGenerationRewatch Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance ChessGenerationRewatch already exists!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartChessGenerationRewatch(string fen)
    {
        for (int i = 0; i < whiteChess.Count; i++)
        {
            _whiteChess[whiteChess[i].chessType] = whiteChess[i].chessPrefab;
            _blackChess[blackChess[i].chessType] = blackChess[i].chessPrefab;
        }
        
        LoadFEN(fen);
    }
    
    private void LoadFEN(string fen)
    {
        string[] spawnPositions = fen.Split(' ')[0].Split('/');
        
        int rowCount = 0;

        foreach (string row in spawnPositions.Reverse())
        {
            int column = 0;
            foreach (char symbol in row)
            {
                
                if (char.IsNumber(symbol))
                {
                    column += (symbol - '0');
                    if (column > 7)
                    {
                        break;
                    }
                }
                else
                {
                    ChessType chessType = Chess.Decoders.DecodeFENChessType(symbol);
                    if (char.IsUpper(symbol))
                    {
                        SpawnChessForRewatch(Chess.Decoders.DecodePositionFromInt(rowCount * 8 + column),
                            chessType, ChessColour.White);}
                    else
                    {
                        SpawnChessForRewatch(Chess.Decoders.DecodePositionFromInt(rowCount * 8 + column),
                            chessType, ChessColour.Black);
                    }

                    column++;
                }
            }

            rowCount++;

        }
    }

    public void SpawnChessForRewatch(string position, ChessType chessType, ChessColour colour)
    {
        GameObject createdChess = PieceSpawn(position, chessType, colour);
        _cells[position].GetComponent<BoardCell>().SetChessExtended(createdChess, chessType, colour);
    }
    
    private void DeactivateAllPieces()
    {
        foreach (GameObject cellWithPiece in _cells.Values)
        {
            BoardCell.SendPiece piece = cellWithPiece.GetComponent<BoardCell>().DisableChess();
            if (piece.hasChess)
            {
                DeactivatePiece(piece.pieceObject, piece.chessType, piece.chessColour);
            }
        }
    }

    public void LoadTurn(string fen)
    {
        DeactivateAllPieces();
        LoadFEN(fen);
    }
}
