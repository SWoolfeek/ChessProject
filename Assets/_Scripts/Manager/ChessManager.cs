using System.Collections.Generic;
using Chess;
using Unity.Mathematics;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
    [SerializeField] private BoardParameters boardParameters;

    [Header("Chess Pick colours")] 
    [SerializeField] private Color possibleTurnColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color possibleTurnEmission;
    [SerializeField] private Color possibleEliminationColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color possibleEliminationEmission;
    
    public ChessManager Instance { get; private set; }

    private Dictionary<string, GameObject> _cells;
    
    
    private string _pickedChessName;
    private List<string> _calculatedPath;

    private List<GameObject> _activePossibleTurnObjects = new List<GameObject>();
    private List<GameObject> _inactivePossibleTurnObjects = new List<GameObject>();

    private bool _pickedChess;
    private int _pickedChessPosition;
    
    private Chess.MoveGenerator _moveGenerator;
    private Dictionary<int, Chess.Move[]> _moves;

    private void Awake()
    {
        if (Instance != null)
        {
           Debug.LogError("Instance ChessManager already exists!");
           Destroy(gameObject);
           return;
        }

        Instance = this;
        
        GlobalGameEventManager.OnCellChooseEvent.AddListener(CellPicked);
    }

    // Start is called before the first frame update
    public void StartChessManager()
    {
        _moveGenerator = new MoveGenerator();
        _moves = _moveGenerator.GenerateLegalMoves();
    }

    public void SetBoard(Dictionary<string, GameObject> input)
    {
        _cells = input;
    }

    private void CellPicked(string cell)
    {
        DisableAllPossibleTurns();
        
        int cellPosition = Decoders.DecodePositionToInt(cell);
        int piece = PrecomputedMoveData.BoardRepresentation.board[cellPosition];
        
        Debug.Log(piece);
        
        if (piece != 0 && Decoders.DecodeBinaryChessColour(piece) == GlobalGameVariables.ChessTurn && _moves.ContainsKey(cellPosition))
        {
            _pickedChessPosition = cellPosition;
            _pickedChess = true;
            foreach (Move move in _moves[cellPosition])
            {
                ShowPossibleTurn(move);
            }
        }
        else if (_pickedChess)
        {
            bool possibleToMove = false;
            Move moveInfo = new Move();
            foreach (Move move in _moves[_pickedChessPosition])
            {
                if (move.TargetPosition == cellPosition)
                {
                    possibleToMove = true;
                    moveInfo = move;
                }
            }

            if (possibleToMove)
            {
                _cells[Decoders.DecodePositionFromInt(_pickedChessPosition)].GetComponent<BoardCell>().MoveTo(_cells[cell]);
                PrecomputedMoveData.BoardRepresentation.MovePiece(_pickedChessPosition,cellPosition);
                
                
                if (moveInfo.Castling)
                {
                    if (moveInfo.TargetPosition > moveInfo.StartPosition)
                    {
                        _cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition + 1)].GetComponent<BoardCell>().MoveTo(_cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition - 1)]);
                    }
                    else
                    {
                        _cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition - 1)].GetComponent<BoardCell>().MoveTo(_cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition + 1)]);
                    }
                }
                else if(moveInfo.EnPassantCapture > -1)
                {
                    _cells[
                            Decoders.DecodePositionFromInt(moveInfo.EnPassantCapture)]
                        .GetComponent<BoardCell>().DestroyChess();
                }
                
                GameManager.Instance.TurnMade();
                _moves = _moveGenerator.GenerateLegalMoves();
            }
            
            DisableAllPossibleTurns();
            _pickedChess = false;
        }
    }

    private void PossibleTurnColor(string position, GameObject possiblePositionObject, ChessType chessMoving)
    {
        bool[] checkResult = GameManager.CheckChess(GlobalGameVariables.ChessTurn, position);
        
        Material material = possiblePositionObject.transform.GetComponentsInChildren<Transform>(true)[1]
            .GetComponent<MeshRenderer>().material;

        if (checkResult[0] && !checkResult[1])
        {
            material.color = possibleEliminationColor;
            material.SetColor("_Emission", possibleEliminationEmission);
        }
        else if ( PrecomputedMoveData.BoardRepresentation.enPassantCapturePosition > -1 && chessMoving == ChessType.Pawn)
        {
            if (position == Decoders.DecodePositionFromInt(PrecomputedMoveData.BoardRepresentation.enPassantCapturePosition))
            {
                material.color = possibleEliminationColor;
                material.SetColor("_Emission", possibleEliminationEmission);
            }
            else
            {
                material.color = possibleTurnColor;
                material.SetColor("_Emission", possibleTurnEmission);
            }
        }
        else
        {
            material.color = possibleTurnColor;
            material.SetColor("_Emission", possibleTurnEmission);
        }
    }

    private void ShowPossibleTurn(Move movement)
    {
        string cellPosition = Decoders.DecodePositionFromInt(movement.TargetPosition);
        
        if (_inactivePossibleTurnObjects.Count > 0)
        {
            _inactivePossibleTurnObjects[0].transform.position = _cells[cellPosition].transform.position;
            _inactivePossibleTurnObjects[0].SetActive(true);
            
            _activePossibleTurnObjects.Add(_inactivePossibleTurnObjects[0]);

            PossibleTurnColor(cellPosition, _inactivePossibleTurnObjects[0], Decoders.DecodeBinaryChessType(PrecomputedMoveData.BoardRepresentation.board[movement.StartPosition]));
            
            _inactivePossibleTurnObjects.RemoveAt(0);
        }
        else
        {
            GameObject newPossibleTurnObject = Instantiate(boardParameters.possibleTurnPrefab, _cells[cellPosition].transform.position,quaternion.identity);
            
            _activePossibleTurnObjects.Add(newPossibleTurnObject);
        }
    }

    private void DisableAllPossibleTurns()
    {
        int clearAmount = _activePossibleTurnObjects.Count;
        if (clearAmount > 0)
        {
            for (int i = 0; i < clearAmount; i++)
            {
                _activePossibleTurnObjects[0].SetActive(false);
                _inactivePossibleTurnObjects.Add(_activePossibleTurnObjects[0]);
                _activePossibleTurnObjects.RemoveAt(0);
            }
        }
        
    }

    private void OnDestroy()
    {
        GlobalGameEventManager.OnCellChooseEvent.RemoveListener(CellPicked);
    }
}
