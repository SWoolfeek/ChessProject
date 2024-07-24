using System;
using System.Collections;
using System.Collections.Generic;
using Chess;
using UnityEngine;
using UnityEngine.UI;

public class BoardCell : MonoBehaviour
{
    private GameObject _chess;
    private bool _hasChess;
    private ChessColour _pieceColour;
    private ChessType _chessType;
    private bool _extended = false;

    private void Awake()
    {
    }

    public void SetChess(GameObject inputChess)
    {
        _chess = inputChess;
        _hasChess = true;

        int piece = PrecomputedMoveData.BoardRepresentation.board[Decoders.DecodePositionToInt(name)];

        _pieceColour = Decoders.DecodeBinaryChessColour(piece);
        _chessType = Decoders.DecodeBinaryChessType(piece);
    }

    public void SetChessExtended(GameObject inputChess, ChessType chessType, ChessColour chessColour)
    {
        _chess = inputChess;
        _hasChess = true;
        _pieceColour = chessColour;
        _chessType = chessType;
        _extended = true;
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
            ChessGenerationManagerGame.Instance.DeactivatePiece(_chess, _chessType, _pieceColour);
        }
    }

    public SendPiece DisableChess()
    {
        if (_hasChess)
        {
            _hasChess = false;
            _chess.SetActive(false);
            return new SendPiece(true, _chess, _chessType, _pieceColour);
        }
        return new SendPiece(false, _chess);
    }

    private void OnMouseDown()
    {
        if (!_extended)
        {
            GlobalGameEventManager.ChooseCell(name);
        }
    }
    
    public class SendPiece
    {
        public bool hasChess = false;
        public GameObject pieceObject;
        public ChessType chessType;
        public ChessColour chessColour;

        public SendPiece(bool inputHasChess, GameObject inputPieceObject, ChessType inputChessType = ChessType.Pawn, ChessColour inputChessColour = ChessColour.White)
        {
            hasChess = inputHasChess;
            pieceObject = inputPieceObject;
            chessType = inputChessType;
            chessColour = inputChessColour;
        }
    }
}
