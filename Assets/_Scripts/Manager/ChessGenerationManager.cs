using System;
using System.Collections;
using System.Collections.Generic;
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
        
        BaseChessPosition();
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
}

[Serializable]
public class ChessPiecePrefabContainer
{
    public ChessType chessType;
    public GameObject chessPrefab;
}
