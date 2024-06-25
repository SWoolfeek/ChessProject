using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessKing : ChessPiece
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
        
        bool[] existChessResultElimination;
        
        
        if ((row + 1) < (boardParameters.gridSize + 1))
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column] + (row + 1).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column] + (row + 1).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column] + (row + 1).ToString());
            }
        }
        
        if ((row - 1) > 0)
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column] + (row - 1).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column] + (row - 1).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column] + (row - 1).ToString());
            }
        }
        
        if ((column - 1) > -1)
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column - 1] + (row).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column - 1] + (row).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column - 1] + (row).ToString());
            }
        }if ((column + 1) < boardParameters.gridSize)
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column + 1] + (row).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column + 1] + (row).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column + 1] + (row).ToString());
            }
        }
        if ((row + 1) < (boardParameters.gridSize + 1) && (column + 1) < (boardParameters.gridSize))
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column + 1] + (row + 1).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column + 1] + (row + 1).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column + 1] + (row + 1).ToString());
            }
        }
        
        if ((row - 1) > 0 && (column + 1) < (boardParameters.gridSize))
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column + 1] + (row - 1).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column + 1] + (row - 1).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column + 1] + (row - 1).ToString());
            }
        }
        
        if ((row + 1) < (boardParameters.gridSize + 1) && (column - 1) > -1)
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column - 1] + (row + 1).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column - 1] + (row + 1).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column - 1] + (row + 1).ToString());
            }
        }
        if ((row - 1) > 0 && (column - 1) > -1)
        {
            existChessResultElimination = GameManager.CheckChess(team, Letters[column - 1] + (row - 1).ToString());
            if (existChessResultElimination[0])
            {
                if (!existChessResultElimination[1])
                {
                    positionToMove.Add(Letters[column - 1] + (row - 1).ToString());
                }
            }
            else
            {
                positionToMove.Add(Letters[column - 1] + (row - 1).ToString());
            }
        }
    }

    private void CalculateCheck()
    {
        
    }
    
}
