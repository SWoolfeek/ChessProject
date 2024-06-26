using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class MoveGenerator
    {
        private List<Move> _moves;

        public List<Move> GenerateMoves()
        {
            for (int startPosition = 0; startPosition < 64; startPosition++)
            {
                int piece= Board.board[startPosition];
                if (Decoders.DecodeBinaryChessColour(piece) == GlobalGameVariables.ChessTurn)
                {
                    GenerateSlidingMoves(startPosition, piece);
                }
            }

            return _moves;
        }

        private void GenerateSlidingMoves(int startingPosition, int piece)
        {
            for (int directionIndex = 0; directionIndex < 8; directionIndex++)
            {
                for (int n = 0; n < PrecomputedMoveData.NumCellsToEdge[startingPosition][directionIndex]; n++)
                {
                    int targetCell = startingPosition + PrecomputedMoveData.DirectionOffsets[directionIndex] * (n + 1);
                    int pieceOnTargetCell = Board.board[targetCell];

                    if (Decoders.DecodeBinaryChessColour(piece) == GlobalGameVariables.ChessTurn)
                    {
                        break;
                    }
                    
                    _moves.Add( (new Move(startingPosition, targetCell)));
                    
                    if (Decoders.DecodeBinaryChessColour(piece) != GlobalGameVariables.ChessTurn)
                    {
                        break;
                    }
                }
            }
        }
    }
}

