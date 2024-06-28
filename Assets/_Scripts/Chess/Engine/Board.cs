using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Board
    {
        public int[] board;
        
        public PieceList[] pawns;
        public PieceList[] bishops;
        public PieceList[] rooks;
        public PieceList[] knights;
        public PieceList[] queens;

        public Board()
        {
            ReInitialize();
        }

        public void ReInitialize()
        {
            board = new int[64];
            
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
                    // King Logic.
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
                        break;
                    case ChessType.Knight:
                        knights[1 - chessTeam].RemovePiece(targetPosition);;
                        break;
                    case ChessType.Bishop:
                        bishops[1 - chessTeam].RemovePiece(targetPosition);;
                        break;
                    case ChessType.Rook:
                        rooks[1 - chessTeam].RemovePiece(targetPosition);;
                        break;
                    case ChessType.Queen:
                        queens[1 - chessTeam].RemovePiece(targetPosition);;
                        break;
                    case ChessType.King:
                        // King Logic.
                        break;
                }
                
                BasicMoving(startingPosition, targetPosition, chessTeam, pieceMoving);
                
            }
            else
            {
                BasicMoving(startingPosition, targetPosition, chessTeam, pieceMoving);
            }
            
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
                    // King Logic.
                    break;
            }
        }
    }
}

