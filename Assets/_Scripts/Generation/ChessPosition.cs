using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessData
{
    public List<List<ChessTurn>> chessTurns = new List<List<ChessTurn>>();
}

public class ChessTurn
{
    public int turnNumber { get; private set; }
    public ChessColour team { get; private set; }
    public List<ChessPosition> chessPosition { get; private set; }

    public void SetChessTurn(int turn, ChessColour teamColour, List<ChessPosition> position)
    {
        turnNumber = turn;
        team = teamColour;
        chessPosition = position;
    }
}

public class ChessPosition
{
    public ChessType chess;
    public string position;
}

public class ChessOnGridPosition
{
    [Min(0)] public int row;
    [Min(0)] public int column;

    public bool PositionEqual(ChessOnGridPosition input)
    {
        if (input.row == row && input.column == column)
        {
            return true;
        }

        return false;

    }

    public Vector3 ToWorldPosition(float cellSize)
    {
        return new Vector3(row * cellSize, 0, column * cellSize);
    }
}

public enum ChessColour
{
    White = 0,
    Black = 1
}

public enum ChessType
{
    Pawn = 0,
    Bishop = 1,
    Knight = 2,
    Rook = 3,
    Queen = 4,
    King = 5
}
