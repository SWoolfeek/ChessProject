using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalGameEventManager : MonoBehaviour
{
    public static CellChooseEvent OnCellChooseEvent = new CellChooseEvent();
    public static ChesChooseEvent OnChesChooseEvent = new ChesChooseEvent();

    public static void ChooseCell(string cell)
    {
        OnCellChooseEvent.Invoke(cell);
    }
    
    public static void ChooseChess(string cell, List<string> path)
    {
        OnChesChooseEvent.Invoke(cell, path);
    }
}

public class CellChooseEvent: UnityEvent<string>{}
public class ChesChooseEvent: UnityEvent<string, List<string>>{}