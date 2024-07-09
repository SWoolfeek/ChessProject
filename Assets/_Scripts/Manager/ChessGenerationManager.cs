using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chess;
using UnityEngine;

public class ChessGenerationManager : MonoBehaviour
{
    [Header("Chess")]
    [SerializeField] private List<ChessPiecePrefabContainer> whiteChess;
    [SerializeField] private List<ChessPiecePrefabContainer> blackChess;
    
    private ChessData rozkladka;
    private Dictionary<string, GameObject> _cells;

    private Dictionary<ChessType, GameObject> _whiteChess = new Dictionary<ChessType, GameObject>();
    private Dictionary<ChessType, GameObject> _blackChess = new Dictionary<ChessType, GameObject>();    
    
    private Dictionary<ChessType, List<GameObject>> _whiteChessInactive = new Dictionary<ChessType, List<GameObject>>()
    {
        { ChessType.Pawn , new List<GameObject>()},
        { ChessType.King , new List<GameObject>()},
        { ChessType.Queen , new List<GameObject>()},
        { ChessType.Knight , new List<GameObject>()},
        { ChessType.Rook , new List<GameObject>()},
        { ChessType.Bishop , new List<GameObject>()}
    };
    private Dictionary<ChessType, List<GameObject>> _blackChessInactive = new Dictionary<ChessType, List<GameObject>>() 
    {
        { ChessType.Pawn , new List<GameObject>()},
        { ChessType.King , new List<GameObject>()},
        { ChessType.Queen , new List<GameObject>()},
        { ChessType.Knight , new List<GameObject>()},
        { ChessType.Rook , new List<GameObject>()},
        { ChessType.Bishop , new List<GameObject>()}
    };
    
    public static ChessGenerationManager Instance { get; private set; }
    
    private const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance ChessGenerationManager already exists!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    // Start is called before the first frame update
    public void StartChessGenerationManager()
    {
        for (int i = 0; i < whiteChess.Count; i++)
        {
            _whiteChess[whiteChess[i].chessType] = whiteChess[i].chessPrefab;
            _blackChess[blackChess[i].chessType] = blackChess[i].chessPrefab;
        }

        StartingFEN(StartFen);
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

    private GameObject PieceSpawn( string position, ChessType chessType, ChessColour colour)
    {
        GameObject result;
        if (colour == ChessColour.White)
        {
            if (_whiteChessInactive[chessType].Count > 0)
            {
                _whiteChessInactive[chessType][0].SetActive(true);
                result = _whiteChessInactive[chessType][0];
                result.transform.parent = _cells[position].transform;
                _whiteChessInactive[chessType].RemoveAt(0);
                return result;
            }
            
            return Instantiate(_whiteChess[chessType], _cells[position].transform);
        }
        else
        {
            if (_blackChessInactive[chessType].Count > 0)
            {
                _blackChessInactive[chessType][0].SetActive(true);
                result = _blackChessInactive[chessType][0];
                result.transform.parent = _cells[position].transform;
                _blackChessInactive[chessType].RemoveAt(0);
                return result;
            }
            
            return Instantiate(_blackChess[chessType], _cells[position].transform);
        }
    }

    private void BaseChessPosition()
    {
        Dictionary<string, ChessType> positions;
        BasicChessPosition basicChess = new BasicChessPosition();
        positions = basicChess.BasicWhitePositions();

        foreach (string position in positions.Keys)
        {
            SpawnChess(position, positions[position], ChessColour.White);
        }
        
        positions = basicChess.BasicBlackPositions();

        foreach (string position in positions.Keys)
        {
            SpawnChess(position, positions[position], ChessColour.Black);
        }

    }

    private void GenerateChessFromChessEngine()
    {
        for (int i = 0; i < Chess.PrecomputedMoveData.BoardRepresentation.board.Length; i++)
        {
            SpawnDecrypt(i, Chess.PrecomputedMoveData.BoardRepresentation.board[i]);
        }
    }

    private void SpawnDecrypt(int position, int piece)
    {
        if (piece != 0)
        {
            SpawnChess(Chess.Decoders.DecodePositionFromInt(position), Chess.Decoders.DecodeBinaryChessType(piece), Chess.Decoders.DecodeBinaryChessColour(piece));
        }
    }

    
    private void StartingFEN(string fen)
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
