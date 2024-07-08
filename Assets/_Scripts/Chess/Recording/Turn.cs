using System.Collections;
using System.Collections.Generic;
using Chess;
using UnityEngine;

namespace Recording
{
    public class Turn
    {
        private readonly Dictionary<int, char> IntToFen = new Dictionary<int, char> 
        {
            { 9, 'K' }, 
            { 10, 'P' },
            { 11, 'N' },
            { 12, 'B' },
            { 13, 'R' },
            { 14, 'Q' },
            { 17, 'k' }, 
            { 18, 'p' },
            { 19, 'n' },
            { 20, 'b' },
            { 21, 'r' },
            { 22, 'q' }
        };
        
        public readonly string FEN;
        public readonly string boardFEN;
        public readonly char teamTurn;
        public readonly string possibleCastling;
        public readonly string possibleEnPassant;
        public readonly int halfTurnToDraw;
        public readonly int fullTurn;
        
        

        public Turn(int inputHalfTurnToDraw, int inputFullTurn)
        {
            boardFEN = ConvertBoardToFEN();
            teamTurn = GlobalGameVariables.ChessTurn.ToString().ToLower()[0];
            possibleCastling = ConvertCastlingToFen();
            possibleEnPassant = ConvertEnPassant();
            halfTurnToDraw = inputHalfTurnToDraw;
            fullTurn = inputFullTurn;
            FEN = boardFEN + " " + teamTurn + " " + possibleCastling + " " + possibleEnPassant + " " + halfTurnToDraw +
                  " " + fullTurn;
        }

        private string ConvertEnPassant()
        {
            if (PrecomputedMoveData.BoardRepresentation.enPassantCapturePosition != -99)
            {
                return Decoders.DecodePositionFromInt(PrecomputedMoveData.BoardRepresentation.enPassantCapturePosition)
                    .ToLower();
            }

            return "-";
        }

        private string ConvertCastlingToFen()
        {
            string result = "";
            bool[][] possibleCastlings = PrecomputedMoveData.BoardRepresentation.possibleCastlings;

            if (possibleCastlings[0][0])
            {
                result += "K";
            }

            if (possibleCastlings[0][1])
            {
                result += "Q";
            }

            if (possibleCastlings[1][0])
            {
                result += "k";
            }

            if (possibleCastlings[1][1])
            {
                result += "q";
            }

            if (result == "")
            {
                result = "-";
            }

            return result;
        }

        private string ConvertBoardToFEN()
        {
            int[] board = PrecomputedMoveData.BoardRepresentation.board;

            List<string> fen = new List<string>();
            string row = "";
            int toSkip = 0;

            for (int i = 0; i < 64; i++)
            {
                if (board[i] == 0)
                {
                    toSkip++;
                }
                else
                {
                    if (toSkip > 0)
                    {
                        row += toSkip;
                        toSkip = 0; 
                    }

                    row += IntToFen[board[i]];
                }
                
                if ((i + 1) %8 == 0)
                {
                    if (toSkip > 0)
                    {
                        row += toSkip;
                    }
                    
                    fen.Add(row);
                    row = "";
                    toSkip = 0;
                }
                
                
            }

            string result = "";
            fen.Reverse();

            for (int i = 0; i < fen.Count; i++)
            {
                result += fen[i];
                
                if (i != fen.Count - 1)
                {
                    result += '/';
                }
            }

            return result;
        }
    }
}

