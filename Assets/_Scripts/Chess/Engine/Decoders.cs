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
            ChessType type = ChessType.Pawn;
            if ((input & Piece.King) == Piece.King)
            {
                type = ChessType.King;
            }
            else if ((input & Piece.Pawn) == Piece.Pawn)
            {
                type = ChessType.Pawn;
            }
            else if ((input & Piece.Knight) == Piece.Knight)
            {
                type = ChessType.Knight;
            }
            else if ((input & Piece.Bishop) == Piece.Bishop)
            {
                type = ChessType.Bishop;
            }
            else if ((input & Piece.Rook) == Piece.Rook)
            {
                type = ChessType.Rook;
            }
            else if ((input & Piece.Queen) == Piece.Queen)
            {
                type = ChessType.Queen;
            }

            return type;
        }

        public static ChessColour DecodeBinaryChessColour(int input)
        {
            if ((input & Piece.White) == Piece.White)
            {
                return ChessColour.White;
            }

            return ChessColour.Black;
            ;
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
            int column = Letters.IndexOf(position[0]);

            return row * 8 + column;
        }
    }
}
