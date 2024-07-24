using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chess;
using UnityEngine;

public class ChessGenerationManager : MonoBehaviour
{
    [Header("Chess")]
    [SerializeField] protected List<ChessPiecePrefabContainer> whiteChess;
    [SerializeField] protected List<ChessPiecePrefabContainer> blackChess;
    
    protected ChessData rozkladka;
    protected Dictionary<string, GameObject> _cells;

    protected Dictionary<ChessType, GameObject> _whiteChess = new Dictionary<ChessType, GameObject>();
    protected Dictionary<ChessType, GameObject> _blackChess = new Dictionary<ChessType, GameObject>();    
    
    protected Dictionary<ChessType, List<GameObject>> _whiteChessInactive = new Dictionary<ChessType, List<GameObject>>()
    {
        { ChessType.Pawn , new List<GameObject>()},
        { ChessType.King , new List<GameObject>()},
        { ChessType.Queen , new List<GameObject>()},
        { ChessType.Knight , new List<GameObject>()},
        { ChessType.Rook , new List<GameObject>()},
        { ChessType.Bishop , new List<GameObject>()}
    };
    protected Dictionary<ChessType, List<GameObject>> _blackChessInactive = new Dictionary<ChessType, List<GameObject>>() 
    {
        { ChessType.Pawn , new List<GameObject>()},
        { ChessType.King , new List<GameObject>()},
        { ChessType.Queen , new List<GameObject>()},
        { ChessType.Knight , new List<GameObject>()},
        { ChessType.Rook , new List<GameObject>()},
        { ChessType.Bishop , new List<GameObject>()}
    };
    
    
    protected const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
    // Start is called before the first frame update
    public void StartChessGenerationManager(bool load, string fen = "")
    {
        for (int i = 0; i < whiteChess.Count; i++)
        {
            _whiteChess[whiteChess[i].chessType] = whiteChess[i].chessPrefab;
            _blackChess[blackChess[i].chessType] = blackChess[i].chessPrefab;
        }

        if (load)
        {
            StartingFEN(fen);
        }
        else
        {
            StartingFEN(StartFen);
        }
        
    }
    
    public void SetBoard(Dictionary<string, GameObject> input)
    {
        _cells = input;
    }

    public void DeactivatePiece(GameObject piece, ChessType chessType, ChessColour colour)
    {
        switch (colour)
        {
            case ChessColour.White:
                _whiteChessInactive[chessType].Add(piece);
                break;
            case ChessColour.Black:
                _blackChessInactive[chessType].Add(piece);
                break;
        }
    }
    
    public void SpawnChess(string position, ChessType chessType, ChessColour colour)
    {
        GameObject createdChess = PieceSpawn(position, chessType, colour);
        _cells[position].GetComponent<BoardCell>().SetChess(createdChess);
    }

    protected GameObject PieceSpawn( string position, ChessType chessType, ChessColour colour)
    {
        GameObject result;
        if (colour == ChessColour.White)
        {
            if (_whiteChessInactive[chessType].Count > 0)
            {
                
                result = _whiteChessInactive[chessType][0];
                _whiteChessInactive[chessType].RemoveAt(0);
                result.SetActive(true);
                result.transform.parent = _cells[position].transform;
                result.transform.localPosition = Vector3.zero;
                return result;
            }
            
            return Instantiate(_whiteChess[chessType], _cells[position].transform);
        }
        else
        {
            if (_blackChessInactive[chessType].Count > 0)
            {
                
                result = _blackChessInactive[chessType][0];
                _blackChessInactive[chessType].RemoveAt(0);
                result.SetActive(true);
                result.transform.parent = _cells[position].transform;
                result.transform.localPosition = Vector3.zero;
                return result;
            }
            
            return Instantiate(_blackChess[chessType], _cells[position].transform);
        }
    }
    
    protected void StartingFEN(string fen)
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
                    for (int i = 0; i < (symbol - '0'); i++)
                    {
                        PrecomputedMoveData.BoardRepresentation.board[rowCount * 8 + column + i] = Piece.None;
                    }
                    
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
                        SpawnChess(Chess.Decoders.DecodePositionFromInt(rowCount * 8 + column),
                            chessType, ChessColour.White);
                        PrecomputedMoveData.BoardRepresentation.AddPiece(rowCount * 8 + column ,chessType, ChessColour.White);
                    }
                    else
                    {
                        SpawnChess(Chess.Decoders.DecodePositionFromInt(rowCount * 8 + column),
                            chessType, ChessColour.Black);
                        PrecomputedMoveData.BoardRepresentation.AddPiece(rowCount * 8 + column ,chessType, ChessColour.Black);
                    }

                    column++;
                }
            }

            rowCount++;

        }
    }
    
}

[Serializable]
public class ChessPiecePrefabContainer
{
    public ChessType chessType;
    public GameObject chessPrefab;
}
