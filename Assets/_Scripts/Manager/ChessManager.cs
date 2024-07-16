using System.Collections.Generic;
using Chess;
using Recording;
using Unity.Mathematics;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
    [SerializeField] private BoardParameters boardParameters;
    [SerializeField] private ChessGenerationManager chessGenerationManager;

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

    private bool _promotionMade;
    private bool _capturedChess;
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
        _moves = _moveGenerator.GenerateLegalMoves(GlobalGameVariables.ChessTurn);
    }

    public void SetBoard(Dictionary<string, GameObject> input)
    {
        _cells = input;
    }

    public void GenerateNextMovements(int halfTurn, int lastCaptureTurn)
    {
        _moves = _moveGenerator.GenerateLegalMoves(GlobalGameVariables.ChessTurn);

        Recording.Turn turn = new Turn(halfTurn - lastCaptureTurn,halfTurn/2);
        
        Game.Instance.AddTurn(halfTurn, turn);
        
        Debug.Log(turn.FEN);

        if (_moves.Keys.Count < 1)
        {
            GameManager.Instance.TeamLoosed();
        }
    }

    private void CellPicked(string cell)
    {
        DisableAllPossibleTurns();
        
        int cellPosition = Decoders.DecodePositionToInt(cell);

        int piece = 0;
        
        if (!_pickedChess && 64 > Decoders.DecodePositionToInt(cell) && Decoders.DecodePositionToInt(cell) > -1 )
        {
            piece = PrecomputedMoveData.BoardRepresentation.board[cellPosition];
        }
        
       
        
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
                if (move.TargetPosition == cellPosition || (cell == PawnPromotionPosition(move) && move.Promotion))
                {
                    possibleToMove = true;
                    moveInfo = move;
                }
            }

            if (possibleToMove)
            {
                if (moveInfo.Promotion)
                {
                    _promotionMade = true;
                }
                else
                {
                    if (PrecomputedMoveData.BoardRepresentation.board[Decoders.DecodePositionToInt(cell)] > 0)
                    {
                        _capturedChess = true;
                    }
                    
                    _cells[Decoders.DecodePositionFromInt(_pickedChessPosition)].GetComponent<BoardCell>()
                        .MoveTo(_cells[cell]);
                    PrecomputedMoveData.BoardRepresentation.MovePiece(_pickedChessPosition, cellPosition);


                    if (moveInfo.Castling)
                    {
                        if (moveInfo.TargetPosition > moveInfo.StartPosition)
                        {
                            _cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition + 1)]
                                .GetComponent<BoardCell>()
                                .MoveTo(_cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition - 1)]);
                        }
                        else
                        {
                            _cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition - 1)]
                                .GetComponent<BoardCell>()
                                .MoveTo(_cells[Decoders.DecodePositionFromInt(moveInfo.TargetPosition + 1)]);
                        }
                    }
                    else if (moveInfo.EnPassantCapture > -1)
                    {
                        _cells[
                                Decoders.DecodePositionFromInt(moveInfo.EnPassantCapture)]
                            .GetComponent<BoardCell>().DestroyChess();
                        _capturedChess = true;
                    }
                }

                GameManager.Instance.TurnMade(_promotionMade, _capturedChess, _pickedChessPosition);
                
                _capturedChess = false;
                _promotionMade = false;
            }
            
            DisableAllPossibleTurns();
            _pickedChess = false;
        }
    }

    private string PawnPromotionPosition(Move move)
    {
        int row = Decoders.DecodePositionFromInt(move.TargetPosition)[1] - '0';
        if (move.TargetPosition > 32)
        {
            return Decoders.DecodePositionFromInt(move.TargetPosition)[0] + (row + 1).ToString();
        }
        else
        {
            return Decoders.DecodePositionFromInt(move.TargetPosition)[0] + (row - 1).ToString();
        }
        
    }

    public void PawnPromotion(int position, ChessType chessTypeInput)
    {
        string cell = Decoders.DecodePositionFromInt(position);
        
        _cells[cell].GetComponent<BoardCell>().DestroyChess();
        chessGenerationManager.SpawnChess(cell, chessTypeInput, GlobalGameVariables.ChessTurn);
    }

    private void PossibleTurnColor(string position, GameObject possiblePositionObject, ChessType chessMoving, bool promotion)
    {

        int piece = 0;
        if (Decoders.DecodePositionToInt(position) < 64 && Decoders.DecodePositionToInt(position) > -1)
        {
            piece = PrecomputedMoveData.BoardRepresentation.board[Decoders.DecodePositionToInt(position)];
        }
        
        
        Material material = possiblePositionObject.transform.GetComponentsInChildren<Transform>(true)[1]
            .GetComponent<MeshRenderer>().material;

        if (promotion)
        {
            material.color = possibleTurnColor;
            material.SetColor("_EmissionColor", possibleTurnEmission);
        }
        else if (piece > 0)
        {
            material.color = possibleEliminationColor;
            material.SetColor("_EmissionColor", possibleEliminationEmission);
        }
        else if ( PrecomputedMoveData.BoardRepresentation.enPassantCapturePosition > -1 && chessMoving == ChessType.Pawn)
        {
            if (position == Decoders.DecodePositionFromInt(PrecomputedMoveData.BoardRepresentation.enPassantCapturePosition))
            {
                material.color = possibleEliminationColor;
                material.SetColor("_EmissionColor", possibleEliminationEmission);
            }
            else
            {
                material.color = possibleTurnColor;
                material.SetColor("_EmissionColor", possibleTurnEmission);
            }
        }
        else
        {
            material.color = possibleTurnColor;
            material.SetColor("_EmissionColor", possibleTurnEmission);
        }
    }

    private void ShowPossibleTurn(Move movement)
    {
        string cellPosition = Decoders.DecodePositionFromInt(movement.TargetPosition);
        if (movement.Promotion)
        {
            Debug.Log("Show possible Promotion");
            cellPosition = PawnPromotionPosition(movement);
        }
        
        
        if (_inactivePossibleTurnObjects.Count > 0)
        {
            _inactivePossibleTurnObjects[0].transform.position = _cells[cellPosition].transform.position;
            _inactivePossibleTurnObjects[0].SetActive(true);
            
            _activePossibleTurnObjects.Add(_inactivePossibleTurnObjects[0]);

            PossibleTurnColor(cellPosition, _inactivePossibleTurnObjects[0], Decoders.DecodeBinaryChessType(PrecomputedMoveData.BoardRepresentation.board[movement.StartPosition]), movement.Promotion);
            
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
