using System.Collections;
using System.Collections.Generic;
using Recording;
using UnityEngine;

public class GlobalGameVariables
{
    private static GlobalGameVariables instance;

    public static GlobalGameVariables Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GlobalGameVariables();
            }

            return instance;
        }
    }

    public static ChessColour ChessTurn = ChessColour.White;
    public static string GameId;
    public static GameEndings gameStatus = GameEndings.Unfinished;
}
