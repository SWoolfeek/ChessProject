using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRook : ChessPiece
{
    void Awake()
    {
        calculateMovement = CalculateMovement;
    }

    public void CalculateMovement()
    {
        positionToMove = new List<string>();
        int column = Letters.IndexOf(_position[0]);
        int row = _position[1] - '0';

        bool up = true;
        bool down = true;
        bool left = true;
        bool right = true;
        
        bool[] existChessResultElimination;
        
        for (int i = 0; i < boardParameters.gridSize; i++)
        {
            
            if ((row + (1 + i)) < (boardParameters.gridSize + 1) && up)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column] + (row + (1 + i)).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column] + (row + (1 + i)).ToString());
                    }

                    up = false;
                }
                else
                {
                    positionToMove.Add(Letters[column] + (row + (1 + i)).ToString());
                }
            }
            
            if ((row - (1 + i)) > 0 && down)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column] + (row - (1 + i)).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column] + (row - (1 + i)).ToString());
                    }

                    down = false;
                }
                else
                {
                    positionToMove.Add(Letters[column] + (row - (1 + i)).ToString());
                }
            }
            
            if ((column - (1 + i)) > -1 && left)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column - (1 + i)] + (row).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column - (1 + i)] + (row).ToString());
                    }

                    left = false;
                }
                else
                {
                    positionToMove.Add(Letters[column - (1 + i)] + (row).ToString());
                }
            }if ((column + (1 + i)) < boardParameters.gridSize && right)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column + (1 + i)] + (row).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column + (1 + i)] + (row).ToString());
                    }

                    right = false;
                }
                else
                {
                    positionToMove.Add(Letters[column + (1 + i)] + (row).ToString());
                }
            }
        }
    }
}
