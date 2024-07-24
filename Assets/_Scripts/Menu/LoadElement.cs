using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadElement : MonoBehaviour
{
    [Header("Text")] 
    
    [SerializeField] private TMP_Text gameStateText;
    [SerializeField] private TMP_Text dateText;

    private string _saveUId;

    public void SetData(string gameState, string date, string saveUId)
    {
        gameStateText.text = gameState;
        dateText.text = date;
        _saveUId = saveUId;
    }
}
