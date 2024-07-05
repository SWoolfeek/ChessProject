using System.Collections;
using System.Collections.Generic;
using Chess;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GenerateBoardPrefab boardGenerator;

    [Header("Ui Elements")]
    
    [SerializeField] private GameObject uiCamera;

    [SerializeField] private GameObject whitePromotions;
    [SerializeField] private GameObject blackPromotions;
    
    [Header("Managers")]
    
    [SerializeField] private ChessGenerationManager chessGenerationManager;
    [SerializeField] private ChessManager chessManager;

    [Header("Promotions")] 
    
    [SerializeField] private List<Transform> promotionCells;
    
    private Dictionary<string, GameObject> _cells;

    private static Dictionary<string, GameObject> _cellsStat;
    
    private int _turn = 1;
    private int _promotionPosition = 1;

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
        
        chessGenerationManager.StartChessGenerationManager();
        chessManager.StartChessManager();
        
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
    
    // Return 1 - same team, 1 - does chessexist.
    public static bool[] CheckChess(ChessColour team, string position)
    {
        return _cellsStat[position].GetComponent<BoardCell>().HasChess(team);
    }

    public void TurnMade(bool promotionWasMade, int promotionPosition)
    {
        Debug.Log("Turn " + _turn);

        if (promotionWasMade)
        {
            _promotionPosition = promotionPosition;
            Promotion();
        }
        else
        {
            _turn++;
            GlobalGameVariables.ChessTurn = GlobalGameVariables.ChessTurn == ChessColour.White
                ? ChessColour.Black
                : ChessColour.White;
            chessManager.GenerateNextMovements();
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

    private void PromotionEnded()
    {
        uiCamera.SetActive(false);
        whitePromotions.SetActive(false);
        blackPromotions.SetActive(false);
        
        _turn++;
        GlobalGameVariables.ChessTurn = GlobalGameVariables.ChessTurn == ChessColour.White
            ? ChessColour.Black
            : ChessColour.White;
        chessManager.GenerateNextMovements();
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
