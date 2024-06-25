using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] protected ChessColour team;
    [SerializeField] protected BoardParameters boardParameters;

    protected bool _firstTurn = true;
    protected string _position;
    protected const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    protected List<string> positionToMove;
    protected Action calculateMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(string position)
    {
        _position = position;
        _firstTurn = false;
    }


    public void SetChess(string position)
    {
        _position = position;
    }

    public bool CheckTeam(ChessColour inputTeam)
    {
        if (team == inputTeam && !ChessCheck())
        {
            return true;
        }

        return false;
    }

    public List<string> CalculateMovement()
    {
        calculateMovement.Invoke();
        print(positionToMove.Count);
        return positionToMove;
    }

    private bool ChessCheck()
    {
        return false;
    }
}
