using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private bool _pickedChess;
    private string _pickedChessName;
    private List<string> _calculatedPath;

    private List<GameObject> _activePossibleTurnObjects = new List<GameObject>();
    private List<GameObject> _inactivePossibleTurnObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
           Debug.LogError("Instance ChessManager already exists!");
           Destroy(gameObject);
           return;
        }

        Instance = this;
        
        GlobalGameEventManager.OnChesChooseEvent.AddListener(ChessWasChosen);
        GlobalGameEventManager.OnCellChooseEvent.AddListener(CellWasChosen);
    }

    // Start is called before the first frame update
    public void StartChessManager()
    {
        
    }

    public void SetBoard(Dictionary<string, GameObject> input)
    {
        _cells = input;
    }

    private void ChessWasChosen(string cell, List<string> path)
    {
        Debug.Log("With chess = true");
        _pickedChess = true;
        _pickedChessName = cell;
        _calculatedPath = path;

        foreach (string position in path)
        {
            ShowPossibleTurn(position);
        }
    }

    private void CellWasChosen(string cell)
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

        DisablePossibleTurn();

    }

    private void PossibleTurnColor(string position, GameObject possiblePositionObject)
    {
        bool[] checkResult = GameManager.CheckChess(GlobalGameVariables.ChessTurn, position);
        
        Material material = possiblePositionObject.transform.GetComponentsInChildren<Transform>(true)[1]
            .GetComponent<MeshRenderer>().material;

        if (checkResult[0] && !checkResult[1])
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

    private void ShowPossibleTurn(string cellPosition)
    {
        if (_inactivePossibleTurnObjects.Count > 0)
        {
            _inactivePossibleTurnObjects[0].transform.position = _cells[cellPosition].transform.position;
            _inactivePossibleTurnObjects[0].SetActive(true);
            
            _activePossibleTurnObjects.Add(_inactivePossibleTurnObjects[0]);

            PossibleTurnColor(cellPosition, _inactivePossibleTurnObjects[0]);
            
            _inactivePossibleTurnObjects.RemoveAt(0);
        }
        else
        {
            GameObject newPossibleTurnObject = Instantiate(boardParameters.possibleTurnPrefab, _cells[cellPosition].transform.position,quaternion.identity);
            
            _activePossibleTurnObjects.Add(newPossibleTurnObject);
        }
    }

    private void DisablePossibleTurn()
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
        GlobalGameEventManager.OnChesChooseEvent.RemoveListener(ChessWasChosen);
        GlobalGameEventManager.OnCellChooseEvent.RemoveListener(CellWasChosen);
    }
}
