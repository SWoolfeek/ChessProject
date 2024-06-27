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
    
    public static ChessGenerationManager Instance { get; private set; }
    
    private const string StartFen = "rnbqkbnr/pppppppp/8/4Q3/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
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
        //GenerateChessFromChessEngine();
       // BaseChessPosition();
    }
    
    public void SetBoard(Dictionary<string, GameObject> input)
    {
        _cells = input;
    }
    
    private void SpawnChess(string position, ChessType chessType, ChessColour colour)
    {
        GameObject createdChess;
        switch (colour)
        {
            case ChessColour.White:
                createdChess = Instantiate(_whiteChess[chessType], _cells[position].transform);
                _cells[position].GetComponent<BoardCell>().SetChess(createdChess);
                createdChess.GetComponent<ChessPiece>().SetChess(position);
                break;
            case ChessColour.Black:
                createdChess = Instantiate(_blackChess[chessType], _cells[position].transform);
                _cells[position].GetComponent<BoardCell>().SetChess(createdChess);
                createdChess.GetComponent<ChessPiece>().SetChess(position);
                break;
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
        for (int i = 0; i < Chess.Board.board.Length; i++)
        {
            SpawnDecrypt(i, Chess.Board.board[i]);
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
                        Board.board[rowCount * 8 + column + i] = Piece.None;
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
                        Board.board[rowCount * 8 + column] = Decoders.DecodeChessToInt(ChessColour.White, chessType);
                    }
                    else
                    {
                        SpawnChess(Chess.Decoders.DecodePositionFromInt(rowCount * 8 + column),
                            chessType, ChessColour.Black);
                        Board.board[rowCount * 8 + column] = Decoders.DecodeChessToInt(ChessColour.Black, chessType);
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
