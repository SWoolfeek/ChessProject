using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Board
    {
        public int[] board;

        public int[] kingsPosition;
        
        public PieceList[] pawns;
        public PieceList[] bishops;
        public PieceList[] rooks;
        public PieceList[] knights;
        public PieceList[] queens;

        public bool[][] posibleCastlings = // 0 White, 1 Black. 0 Short, 1 Long.
        {
            new bool[] { true, true },
            new bool[] { true, true }
        };

        private ChessType _previouslyCaptured;
        private bool _previousTurnWasCaptured;
        private bool _previousTurnWasCastling;
        
        public Board()
        {
            ReInitialize();
        }

        public void ReInitialize()
        {
            board = new int[64];

            kingsPosition = new int[2];
            
            knights = new PieceList[] { new PieceList (10), new PieceList (10) };
            pawns = new PieceList[] { new PieceList (8), new PieceList (8) };
            rooks = new PieceList[] { new PieceList (10), new PieceList (10) };
            bishops = new PieceList[] { new PieceList (10), new PieceList (10) };
            queens = new PieceList[] { new PieceList (9), new PieceList (9) };
        }

        public void AddPiece(int position, ChessType chessTypeInput, ChessColour chessTeamInput)
        {
            int chessTeam = chessTeamInput == ChessColour.White ? 0 : 1;
            
            switch (chessTypeInput)
            {
                case ChessType.Pawn:
                    pawns[chessTeam].AddPiece(position);
                    break;
                case ChessType.Knight:
                    knights[chessTeam].AddPiece(position);;
                    break;
                case ChessType.Bishop:
                    bishops[chessTeam].AddPiece(position);;
                    break;
                case ChessType.Rook:
                    rooks[chessTeam].AddPiece(position);;
                    break;
                case ChessType.Queen:
                    queens[chessTeam].AddPiece(position);;
                    break;
                case ChessType.King:
                    kingsPosition[chessTeam] = position;
                    break;
            }
            
            board[position] = Decoders.DecodeChessToInt(chessTeamInput, chessTypeInput);
        }

        public void MovePiece(int startingPosition, int targetPosition)
        {
            ChessType pieceMoving = Decoders.DecodeBinaryChessType(board[startingPosition]);
            int chessTeam = Decoders.DecodeBinaryChessColour(board[startingPosition]) == ChessColour.White ? 0 : 1;
            
            if (board[targetPosition] > 0)
            {
                ChessType targetPiece = Decoders.DecodeBinaryChessType(board[targetPosition]);
                switch (targetPiece)
                {
                    case ChessType.Pawn:
                        pawns[1 - chessTeam].RemovePiece(targetPosition);
                        _previouslyCaptured = ChessType.Pawn;
                        break;
                    case ChessType.Knight:
                        knights[1 - chessTeam].RemovePiece(targetPosition);
                        _previouslyCaptured = ChessType.Knight;
                        break;
                    case ChessType.Bishop:
                        bishops[1 - chessTeam].RemovePiece(targetPosition);
                        _previouslyCaptured = ChessType.Bishop;
                        break;
                    case ChessType.Rook:
                        rooks[1 - chessTeam].RemovePiece(targetPosition);
                        _previouslyCaptured = ChessType.Rook;
                        break;
                    case ChessType.Queen:
                        queens[1 - chessTeam].RemovePiece(targetPosition);
                        _previouslyCaptured = ChessType.Queen;
                        break;
                    case ChessType.King:
                        // King Logic.
                        break;
                }
                
                BasicMoving(startingPosition, targetPosition, chessTeam, pieceMoving);
                _previousTurnWasCaptured = true;
                _previousTurnWasCastling = false;

            }
            else if (pieceMoving == ChessType.King && (Math.Abs(startingPosition - targetPosition) == 2 ||
                                                       Math.Abs(startingPosition - targetPosition) == 3))
            {
                MadeCastling(chessTeam, startingPosition, targetPosition);
                _previousTurnWasCaptured = false;
                _previousTurnWasCastling = true;
            }
            else
            {
                BasicMoving(startingPosition, targetPosition, chessTeam, pieceMoving);
                _previousTurnWasCaptured = false;
                _previousTurnWasCastling = false;
            }
            
        }

        private void MadeCastling(int chessTeam, int startingPosition, int targetPosition)
        {
            if (startingPosition > targetPosition)
            {
                Debug.Log("Castling - " + board[targetPosition - 1] );
                MovePiece(targetPosition - 1, targetPosition + 1);
            }
            else
            {
                MovePiece(targetPosition + 1, targetPosition - 1);
            }
            BasicMoving(startingPosition, targetPosition, chessTeam, ChessType.King);
        }

        private void BasicMoving(int startingPosition, int targetPosition, int chessTeam, ChessType pieceMoving)
        {
            board[targetPosition] = board[startingPosition];
            board[startingPosition] = 0;

            switch (pieceMoving)
            {
                case ChessType.Pawn:
                    pawns[chessTeam].MovePiece(startingPosition,targetPosition);
                    break;
                case ChessType.Knight:
                    knights[chessTeam].MovePiece(startingPosition,targetPosition);
                    break;
                case ChessType.Bishop:
                    bishops[chessTeam].MovePiece(startingPosition,targetPosition);
                    break;
                case ChessType.Rook:
                    rooks[chessTeam].MovePiece(startingPosition,targetPosition);
                    break;
                case ChessType.Queen:
                    queens[chessTeam].MovePiece(startingPosition,targetPosition);
                    break;
                case ChessType.King:
                    kingsPosition[chessTeam] = targetPosition;
                    break;
            }
        }

        public void UndoPreviousMovement(int startingPosition, int targetPosition, ChessColour chessTeam)
        {
            
            int team = chessTeam == ChessColour.White ? 0 : 1;
            BasicMoving(targetPosition, startingPosition, team, Decoders.DecodeBinaryChessType(board[targetPosition]));
            
            if (_previousTurnWasCaptured)
            {
                ChessColour enemyTeam = chessTeam == ChessColour.White ? ChessColour.Black : ChessColour.White;
                AddPiece(targetPosition, _previouslyCaptured,enemyTeam);
            }
            else if (_previousTurnWasCastling)
            {
                Debug.Log("Castling");
                if (startingPosition > targetPosition)
                {
                    BasicMoving(targetPosition + 1, targetPosition - 1 , team, Decoders.DecodeBinaryChessType(board[targetPosition]));
                }
                else
                {
                    BasicMoving(targetPosition - 1, targetPosition + 1 , team, Decoders.DecodeBinaryChessType(board[targetPosition]));
                }
            }
        }
    }
}

