using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Recording;
using Settings;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GenerateBoardPrefab boardGenerator;

    [Header("Ui Elements")]

    [SerializeField] private GameObject loseWindow;
    [SerializeField] private TMP_Text resultText;
    
    [Header("Managers")]
    
    [SerializeField] private ChessGenerationManager chessGenerationManager;
    [SerializeField] private ChessManager chessManager;
    [SerializeField] private GameUiManager uiManager;

    [Header("Promotions")] 
    
    [SerializeField] private List<Transform> promotionCells;
    
    [Header("Settings")] 
    
    [SerializeField] private GameSettings gameSettings;
    
    private Dictionary<string, GameObject> _cells;

    private static Dictionary<string, GameObject> _cellsStat;
    
    private int _turn = 0;
    private int _promotionPosition = 1;
    private int _lastCapture = 0;
    private bool _loaded;

    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance GameManager already exists!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _cells = boardGenerator.ReadExistedCells();
        ReadPromotionCells();
        _cellsStat = _cells;
        
        chessGenerationManager.SetBoard(_cells);
        chessManager.SetBoard(_cells);
        
        if (!gameSettings.PreviousGameUnfinished)
        {
            GenerateRandomId idGenerator = new GenerateRandomId();
            GlobalGameVariables.GameId = idGenerator.Generate();
            GlobalGameVariables.ChessTurn = ChessColour.White;
            GlobalGameVariables.gameStatus = GameEndings.Unfinished;

            PrecomputedMoveData.BoardRepresentation = new Board();
            
            Game.Instance.ClearGame();
            //Game.Instance.SaveGame();
            gameSettings.PreviousGameUId = GlobalGameVariables.GameId;
            gameSettings.PreviousGameUnfinished = true;
            gameSettings.SaveSettings();
            
            chessGenerationManager.StartChessGenerationManager(false);
        }
        else
        {
            PrecomputedMoveData.BoardRepresentation = new Board();
            GlobalGameVariables.GameId = gameSettings.PreviousGameUId;
            Game.Instance.LoadGame();
            LoadGame();
        }
        
        chessManager.StartChessManager();
    }

    private void LoadGame()
    {
        Turn lastTurn = Game.Instance.ReadLastTurn();
        chessGenerationManager.StartChessGenerationManager(true, Game.Instance.ReadLastTurn().FEN);
        GlobalGameVariables.ChessTurn = lastTurn.FEN.Split(' ')[1] == "b" ? ChessColour.Black : ChessColour.White;
        if (lastTurn.possibleEnPassant.Length > 1)
        {
            PrecomputedMoveData.BoardRepresentation.LoadEnPassant(lastTurn.possibleEnPassant);
        }
        

        int teamTurnAdd = GlobalGameVariables.ChessTurn == ChessColour.White ? 0 : 1;
        _turn = lastTurn.fullTurn * 2 + teamTurnAdd;
        Debug.Log("Turn - " + _turn);

        _lastCapture = _turn - lastTurn.halfTurnToDraw;
    }

    private void ReadPromotionCells()
    {
        foreach (Transform promotionCellsByTeam in promotionCells)
        {
            Transform[] childrens = promotionCellsByTeam.GetComponentsInChildren<Transform>(true);
            for (int i = 1; i < childrens.Length; i++)
            {
                _cells[childrens[i].name] = childrens[i].gameObject;
            }
        }
    }

    public void TeamLoosed()
    {
        ChessColour winner = GlobalGameVariables.ChessTurn == ChessColour.White ? ChessColour.Black : ChessColour.White;
        int team = GlobalGameVariables.ChessTurn == ChessColour.White ? 0 : 1;
        bool underCheck = false;

        MoveGenerator moveGenerator = new MoveGenerator();
        Dictionary<int, Chess.Move[]> moves = moveGenerator.GenerateLegalMoves(winner);
        
        if (moves.Keys.Count < 1)
        {
            resultText.text = "It is draw.";
            GlobalGameVariables.gameStatus = GameEndings.Draw;
        }
        else
        {
            foreach (int key in moves.Keys)
            {
                if (moves[key].Any(response => response.TargetPosition == PrecomputedMoveData.BoardRepresentation.kingsPosition[team]))
                {
                    underCheck = true;
                    break;
                }
            }

            if (underCheck)
            {
                resultText.text = winner.ToString() + " won.";
                if (winner == ChessColour.White)
                {
                    GlobalGameVariables.gameStatus = GameEndings.White;
                }
                else
                {
                    GlobalGameVariables.gameStatus = GameEndings.Black;
                }
                
            }
            else
            {
                resultText.text = "It is draw.";
                GlobalGameVariables.gameStatus = GameEndings.Draw;
            }
            
        }
        
        gameSettings.PreviousGameUnfinished = false;
        gameSettings.SaveSettings();
        
        uiManager.GameFinished();
        
    }

    public void TurnMade(bool promotionWasMade, bool captureWasMade, int promotionPosition)
    {
        if (captureWasMade)
        {
            _lastCapture = _turn + 1;
        }

        if (promotionWasMade)
        {
            _lastCapture = _turn + 1;
            _promotionPosition = promotionPosition;
            uiManager.Promotion(GlobalGameVariables.ChessTurn);
        }
        else
        {
            EndTurn();
        }
    }

    private void EndTurn()
    {
        _turn++;
        GlobalGameVariables.ChessTurn = GlobalGameVariables.ChessTurn == ChessColour.White
            ? ChessColour.Black
            : ChessColour.White;
        chessManager.GenerateNextMovements(_turn, _lastCapture);
    }

    public void PromotionResult(ChessType inputChessType)
    {
        PrecomputedMoveData.BoardRepresentation.PromotePawn(_promotionPosition,inputChessType , GlobalGameVariables.ChessTurn);
        chessManager.PawnPromotion(_promotionPosition, inputChessType);
        EndTurn();
    }
    
}
