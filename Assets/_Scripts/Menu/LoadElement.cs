using System.Collections;
using System.Collections.Generic;
using Recording;
using TMPro;
using UnityEngine;

public class LoadElement : MonoBehaviour
{
    [Header("Text")] 
    
    [SerializeField] private TMP_Text gameStateText;
    [SerializeField] private TMP_Text dateText;

    private string _saveUId;
    private GameEndings _gameState;

    public void SetData(GameEndings gameState, string date, string saveUId)
    {
        _gameState = gameState;
        gameStateText.text = _gameState.ToString();
        dateText.text = date;
        _saveUId = saveUId;
    }

    public void LoadGame()
    {
        if (_gameState == GameEndings.Unfinished)
        {
            Debug.Log(_saveUId);
            MenuManager.Instance.LoadGame(_saveUId);
        }
        else
        {
            Debug.Log("This game is finished");
            RewatchGame();
        }
    }

    public void RewatchGame()
    {
        
    }
}
