using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] private ChessColour team;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckTeam(ChessColour inputTeam)
    {
        if (team == inputTeam && !ChessCheck())
        {
            return true;
        }

        return false;
    }

    private bool ChessCheck()
    {
        return false;
    }
}
