using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece
{
    private int _regularStep = 1;
    private int _firstStep = 2;
    
    // Start is called before the first frame update
    void Awake()
    {
        calculateMovement = CalculateMovement;
    }

    public void CalculateMovement()
    {
        int step = _regularStep;
        int teamMultiplier = 0;

        switch (team)
        {
           case ChessColour.White:
               teamMultiplier = 1;
               break;
           case ChessColour.Black:
               teamMultiplier = -1;
               break;
        }

        if (_firstTurn)
        {
            step = _firstStep;
        }

        positionToMove = new List<string>();
        
        int column = Letters.IndexOf(_position[0]);
        int row = _position[1] - '0';
        
        for (int i = 0; i < step; i++)
        {
            
            if ((row + (1 + i) * teamMultiplier) < (boardParameters.gridSize + 1) && (row + (1 + i) * teamMultiplier) > 0)
            {
                string positionNext = Letters[column] + (row + (1 + i) * teamMultiplier).ToString();
                
                bool[] existChessResult = GameManager.CheckChess(team, positionNext);

                if (row + (1 + i) * teamMultiplier == row + (1 + 0) * teamMultiplier)
                {
                    bool[] existChessResultElimination;
                    
                    if (column + 1 < boardParameters.gridSize)
                    {
                        existChessResultElimination = GameManager.CheckChess(team, Letters[column + 1] + (row + (1 * teamMultiplier)).ToString());
                        

                        if (existChessResultElimination[0] && !existChessResultElimination[1])
                        {
                            positionToMove.Add(Letters[column + 1] + (row + (1 * teamMultiplier)).ToString());
                        }
                    }

                    if (column - 1 > -1)
                    {
                        existChessResultElimination = GameManager.CheckChess(team, Letters[column - 1] + (row + (1 * teamMultiplier)).ToString());

                        if (existChessResultElimination[0] && !existChessResultElimination[1])
                        {
                            positionToMove.Add(Letters[column - 1] + (row + (1 * teamMultiplier)).ToString());
                        }
                    }
                }
                
                if (!existChessResult[0] && !existChessResult[1]) // If chess exist at this position and from different team.
                {
                    positionToMove.Add(positionNext);
                }
                else
                {
                    break;
                }
                
                
            }
            else
            {
                break;
            }
        }
    }
}
