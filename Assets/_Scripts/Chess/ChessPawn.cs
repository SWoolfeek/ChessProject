using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece
{
    private int _regularStep = 1;
    private int _firstStep = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        calculateMovement = CalculateMovement;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        for (int i = 0; i < step; i++)
        {
            int row = Letters.IndexOf(_position[0]);
            
            if ((row + (1 + i) * teamMultiplier) < (boardParameters.gridSize) && (row + (1 + i) * teamMultiplier) > -1)
            {
                string positionNext = Letters[row + (1 + i) * teamMultiplier] + _position[1].ToString();
                
                bool[] existChessResult = GameManager.CheckChess(team, positionNext);

                if (row + (1 + i) * teamMultiplier == row + (1 + 0) * teamMultiplier)
                {
                    bool[] existChessResultElimination;
                    int columnElimination = _position[1] - '0';
                    
                    if (columnElimination + 1 < boardParameters.gridSize + 1)
                    {
                        existChessResultElimination = GameManager.CheckChess(team, positionNext[0] + (columnElimination + 1).ToString());

                        if (existChessResultElimination[0] && !existChessResultElimination[1])
                        {
                            positionToMove.Add(positionNext[0] + (columnElimination + 1).ToString());
                        }
                    }

                    if (columnElimination - 1 > 0)
                    {
                        existChessResultElimination = GameManager.CheckChess(team, positionNext[0] + (columnElimination - 1).ToString());

                        if (existChessResultElimination[0] && !existChessResultElimination[1])
                        {
                            positionToMove.Add(positionNext[0] + (columnElimination - 1).ToString());
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
