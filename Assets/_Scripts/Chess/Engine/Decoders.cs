using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{

    public static class Decoders
    {
        private const string Letters = "ABCDEFGH";

        public static ChessType DecodeFENChessType(char input)
        {
            input = char.ToLower(input);

            switch (input)
            {
                case 'k':
                    return ChessType.King;
                case 'p':
                    return ChessType.Pawn;
                case 'b':
                    return ChessType.Bishop;
                case 'n':
                    return ChessType.Knight;
                case 'r':
                    return ChessType.Rook;
                case 'q':
                    return ChessType.Queen;
            }

            return ChessType.Pawn;
        }

        public static ChessType DecodeBinaryChessType(int input)
        {
            int colour = Piece.White;;

            if (input > Piece.Black)
            {
                colour = Piece.Black;
            }
            
            ChessType type = ChessType.Pawn;
            if ((input - colour) == Piece.King)
            {
                type = ChessType.King;
            }
            else if ((input - colour) == Piece.Pawn)
            {
                type = ChessType.Pawn;
            }
            else if ((input - colour) == Piece.Knight)
            {
                type = ChessType.Knight;
            }
            else if ((input - colour) == Piece.Bishop)
            {
                type = ChessType.Bishop;
            }
            else if ((input - colour) == Piece.Rook)
            {
                type = ChessType.Rook;
            }
            else if ((input - colour) == Piece.Queen)
            {
                type = ChessType.Queen;
            }

            return type;
        }

        public static ChessColour DecodeBinaryChessColour(int input)
        {
            if (input > Piece.Black)
            {
                return ChessColour.Black;
            }

            return ChessColour.White;
        }

        public static string DecodePositionFromInt(int position)
        {
            int row = position / 8;
            int column = position % 8;

            return Letters[column] + (row + 1).ToString();
        }

        public static int DecodePositionToInt(string position)
        {
            int row = (position[1] - '0') - 1;
            int column = Letters.IndexOf(Char.ToUpper(Char.ToLower(position[0])));

            return row * 8 + column;
        }
        
        public static int DecodeChessToInt(ChessColour team, ChessType type)
        {
            int chessTeam = 0;
            int chessType = 0;

            switch (type)
            {
                case ChessType.Pawn:
                    chessType = Piece.Pawn;
                    break;
                case ChessType.King:
                    chessType = Piece.King;
                    break;
                case ChessType.Queen:
                    chessType = Piece.Queen;
                    break;
                case ChessType.Rook:
                    chessType = Piece.Rook;
                    break;
                case ChessType.Bishop:
                    chessType = Piece.Bishop;
                    break;
                case ChessType.Knight:
                    chessType = Piece.Knight;
                    break;
            }

            switch (team)
            {
                case ChessColour.White:
                    chessTeam = Piece.White;
                    break;
                case ChessColour.Black:
                    chessTeam = Piece.Black;
                    break;
            }

            return chessTeam | chessType;
        }
        
        
    }
}
