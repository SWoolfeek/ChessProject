using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBishop : ChessPiece
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

        bool leftUp = true;
        bool leftDown = true;
        bool rightUp = true;
        bool rightDown = true;
        
        bool[] existChessResultElimination;
        
        for (int i = 0; i < boardParameters.gridSize; i++)
        {
            
            if ((row + (1 + i)) < (boardParameters.gridSize + 1) && (column + (1 + i)) < (boardParameters.gridSize) && rightUp)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column + (1 + i)] + (row + (1 + i)).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column + (1 + i)] + (row + (1 + i)).ToString());
                    }

                    rightUp = false;
                }
                else
                {
                    positionToMove.Add(Letters[column + (1 + i)] + (row + (1 + i)).ToString());
                }
            }
            
            if ((row - (1 + i)) > 0 && (column + (1 + i)) < (boardParameters.gridSize) && rightDown)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column + (1 + i)] + (row - (1 + i)).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column + (1 + i)] + (row - (1 + i)).ToString());
                    }

                    rightDown = false;
                }
                else
                {
                    positionToMove.Add(Letters[column + (1 + i)] + (row - (1 + i)).ToString());
                }
            }
            
            if ((row + (1 + i)) < (boardParameters.gridSize + 1) && (column - (1 + i)) > -1 && leftUp)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column - (1 + i)] + (row + (1 + i)).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column - (1 + i)] + (row + (1 + i)).ToString());
                    }

                    leftUp = false;
                }
                else
                {
                    positionToMove.Add(Letters[column - (1 + i)] + (row + (1 + i)).ToString());
                }
            }
            if ((row - (1 + i)) > 0 && (column - (1 + i)) > -1 && leftDown)
            {
                existChessResultElimination = GameManager.CheckChess(team, Letters[column - (1 + i)] + (row - (1 + i)).ToString());
                if (existChessResultElimination[0])
                {
                    if (!existChessResultElimination[1])
                    {
                        positionToMove.Add(Letters[column - (1 + i)] + (row - (1 + i)).ToString());
                    }

                    leftDown = false;
                }
                else
                {
                    positionToMove.Add(Letters[column - (1 + i)] + (row - (1 + i)).ToString());
                }
            }
        }
    }
}
