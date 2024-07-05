using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GenerateBoardPrefab boardGenerator;

    [Header("Ui Elements")]
    
    [SerializeField] private Camera uiCamera;

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

    public void TurnMade()
    {
        _turn++;
        GlobalGameVariables.ChessTurn = GlobalGameVariables.ChessTurn == ChessColour.White
            ? ChessColour.Black
            : ChessColour.White;
        
        Debug.Log("Turn " + _turn);
    }
}
