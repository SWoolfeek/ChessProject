using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
    [SerializeField] private BoardParameters boardParameters;
    [SerializeField] private GenerateBoardPrefab boardGenerator;

    [SerializeField] private GameObject whitePawn;
    [SerializeField] private GameObject blackPawn;
    
    public static ChessManager Instance { get; private set; }

    private static Dictionary<string, GameObject> _cells;
    
    private Dictionary<int[,], ChessPiece> chess = new Dictionary<int[,], ChessPiece>();

    private ChessPiece _chosenChess;

    private ChessData rozkladka;

    private static bool _pickedChess;
    private static string _pickedChessName;
    private static List<string> _calculatedPath;

    private void Awake()
    {
        if (Instance != null)
        {
           Debug.LogError("Instance ChessManager already exists!");
           Destroy(gameObject);
           return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cells = boardGenerator.ReadExistedCells();
        
        //Manual testing.
        rozkladka = new ChessData();
        
        ChessTurn whiteTurn = new ChessTurn();

        List<ChessPosition> whitePositions = new List<ChessPosition>();

        ChessPosition a1 = new ChessPosition();
        a1.position = "A1";
        a1.chess = ChessType.Pawn;
        whitePositions.Add(a1);
        ChessPosition a2 = new ChessPosition();
        a2.position = "A2";
        a2.chess = ChessType.Pawn;
        whitePositions.Add(a2);
        ChessPosition b1 = new ChessPosition();
        b1.position = "B1";
        b1.chess = ChessType.Pawn;
        whitePositions.Add(b1);
        ChessPosition b2 = new ChessPosition();
        b2.position = "B2";
        b2.chess = ChessType.Pawn;
        whitePositions.Add(b2);
        
        whiteTurn.SetChessTurn(0,ChessColour.White, whitePositions);
        
        ChessTurn blackTurn = new ChessTurn();
        
        List<ChessPosition> blackPositions = new List<ChessPosition>();

        ChessPosition h1 = new ChessPosition();
        h1.position = "H1";
        h1.chess = ChessType.Pawn;
        blackPositions.Add(h1);
        
        ChessPosition h2 = new ChessPosition();
        h2.position = "H2";
        h2.chess = ChessType.Pawn;
        blackPositions.Add(h2);
        ChessPosition g1 = new ChessPosition();
        g1.position = "G1";
        g1.chess = ChessType.Pawn;
        blackPositions.Add(g1);
        ChessPosition g2 = new ChessPosition();
        g2.position = "G2";
        g2.chess = ChessType.Pawn;
        blackPositions.Add(g2);
        
        blackTurn.SetChessTurn(0, ChessColour.Black,blackPositions);
        
        rozkladka.chessTurns.Add(new List<ChessTurn>() {whiteTurn, blackTurn});

        foreach (List<ChessTurn> turn in rozkladka.chessTurns)
        {
            foreach (ChessTurn i in turn)
            {
                foreach (ChessPosition position in i.chessPosition)
                {
                    CreateChess(position, i.team);
                }
            }
        }
            
        //End testing.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateChess(ChessPosition inputPosition, ChessColour colour)
    {
        GameObject createdChess;
        switch (colour)
        {
            case ChessColour.White:
                createdChess = Instantiate(whitePawn, _cells[inputPosition.position].transform);
                _cells[inputPosition.position].GetComponent<BoardCell>().SetChess(createdChess);
                createdChess.GetComponent<ChessPiece>().SetChess(inputPosition.position);
                break;
            case ChessColour.Black:
                createdChess = Instantiate(blackPawn, _cells[inputPosition.position].transform);
                _cells[inputPosition.position].GetComponent<BoardCell>().SetChess(createdChess);
                createdChess.GetComponent<ChessPiece>().SetChess(inputPosition.position);
                break;
        }
    }

    public static void ChessWasChosen(string cell, List<string> path)
    {
        Debug.Log("With chess = true");
        _pickedChess = true;
        _pickedChessName = cell;
        _calculatedPath = path;
    }

    public static void CellWasChosen(string cell)
    {
        if (_pickedChess)
        {
            
            if (_calculatedPath.Contains(cell))
            {
                Debug.Log("move chess");
                _cells[_pickedChessName].GetComponent<BoardCell>().MoveTo(_cells[cell]);
                _pickedChess = false;
            }
            else
            {
                Debug.Log("Wrong path");
            }
            
        }
        else
        {
            Debug.Log("With chess = false");
            _pickedChess = false;
            _pickedChessName = cell;
        }
        
    }

    public void AddChess(Vector3 position, ChessPiece chessController)
    {
        if (!chess.ContainsKey(CalculateBoardPosition(position)))
        {
            chess.Add(CalculateBoardPosition(position), chessController); 
        }
        else
        {
            Debug.LogError("Chess for this position was already added!!!");
        }
        
    }

    public bool CheckChessExisting(Vector3 inputPosition, ChessColour team)
    {
        int[,] boardPosition = CalculateBoardPosition(inputPosition);

        if (chess.ContainsKey(boardPosition))
        {
            if (chess[boardPosition].CheckTeam(team))
            {
                _chosenChess = chess[boardPosition];
                return true;
            }

            return false;

        }

        return false;
    }

    private int[,] CalculateBoardPosition(Vector3 inputPosition)
    {
        int row = (int)inputPosition.x / (int)boardParameters.cellSize;
        int column = (int)inputPosition.y / (int)boardParameters.cellSize;
        int[,] boardPosition = new int[row,column];

        return boardPosition;
    }
}
