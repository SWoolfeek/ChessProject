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
    
    [SerializeField] private GameObject uiCamera;

    [SerializeField] private GameObject whitePromotions;
    [SerializeField] private GameObject blackPromotions;

    [SerializeField] private GameObject loseWindow;
    [SerializeField] private TMP_Text resultText;
    
    [Header("Managers")]
    
    [SerializeField] private ChessGenerationManager chessGenerationManager;
    [SerializeField] private ChessManager chessManager;

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
            
            Game.Instance.SaveGame();
            gameSettings.PreviousGameUId = GlobalGameVariables.GameId;
            gameSettings.PreviousGameUnfinished = true;
            gameSettings.SaveSettings();
            
            chessGenerationManager.StartChessGenerationManager(false);
        }
        else
        {
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
        
        loseWindow.SetActive(true);
        
    }

    public void TurnMade(bool promotionWasMade, bool captureWasMade, int promotionPosition)
    {
        Debug.Log("Turn " + _turn);

        if (captureWasMade)
        {
            _lastCapture = _turn + 1;
        }

        if (promotionWasMade)
        {
            _lastCapture = _turn + 1;
            _promotionPosition = promotionPosition;
            Promotion();
        }
        else
        {
            EndTurn();
        }
        
              
    }

    private void Promotion()
    {
        uiCamera.SetActive(true);
        
        if (GlobalGameVariables.ChessTurn == ChessColour.White)
        {
            whitePromotions.SetActive(true);
        }
        else
        {
            blackPromotions.SetActive(true);
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

    private void PromotionEnded()
    {
        uiCamera.SetActive(false);
        whitePromotions.SetActive(false);
        blackPromotions.SetActive(false);

        EndTurn();
    }

    #region PromotionButtons

    public void QueenPromotion()
    {
        PrecomputedMoveData.BoardRepresentation.PromotePawn(_promotionPosition, ChessType.Queen, GlobalGameVariables.ChessTurn);
        chessManager.PawnPromotion(_promotionPosition, ChessType.Queen);
        PromotionEnded();
    }
    
    public void RookPromotion()
    {
        PrecomputedMoveData.BoardRepresentation.PromotePawn(_promotionPosition, ChessType.Rook, GlobalGameVariables.ChessTurn);
        chessManager.PawnPromotion(_promotionPosition, ChessType.Rook);
        PromotionEnded();
    }
    
    public void BishopPromotion()
    {
        PrecomputedMoveData.BoardRepresentation.PromotePawn(_promotionPosition, ChessType.Bishop, GlobalGameVariables.ChessTurn);
        chessManager.PawnPromotion(_promotionPosition, ChessType.Bishop);
        PromotionEnded();
    }
    
    public void KnightPromotion()
    {
        PrecomputedMoveData.BoardRepresentation.PromotePawn(_promotionPosition, ChessType.Knight, GlobalGameVariables.ChessTurn);
        chessManager.PawnPromotion(_promotionPosition, ChessType.Knight);
        PromotionEnded();
    }

    #endregion
    
}
