using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    private GameObject _chess;
    private bool _hasChess;

    public void SetChess(GameObject inputChess)
    {
        _chess = inputChess;
        _hasChess = true;
    }

    public void MoveTo(GameObject target)
    {
        _chess.transform.position = target.transform.position;
        _chess.transform.parent = target.transform;

        //_chess.GetComponent<ChessPiece>().MoveTo(target.name);
        target.GetComponent<BoardCell>().DestroyChess();
        target.GetComponent<BoardCell>().SetChess(_chess);
        _hasChess = false;
        
    }

    public void DestroyChess()
    {
        if (_hasChess)
        {
            _chess.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        GlobalGameEventManager.ChooseCell(name);
    }
}
