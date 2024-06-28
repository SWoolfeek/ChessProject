using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class MoveGenerator
    {
        private Dictionary<int, Move[]> _moves;

        public Dictionary<int, Move[]> GenerateMoves()
        {
            _moves = new Dictionary<int, Move[]>();
            
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
            int startDirIndex = (Decoders.DecodeBinaryChessType(piece) == ChessType.Bishop) ? 4 : 0;
            int endDirIndex = (Decoders.DecodeBinaryChessType(piece) == ChessType.Rook) ? 4 : 8;
            
            List<Move> targetPositions = new List<Move>();
            for (int directionIndex = startDirIndex; directionIndex < endDirIndex; directionIndex++)
            {
                for (int n = 0; n < PrecomputedMoveData.NumCellsToEdge[startingPosition][directionIndex]; n++)
                {
                    int targetCell = startingPosition + PrecomputedMoveData.DirectionOffsets[directionIndex] * (n + 1);
                    int pieceOnTargetCell = Board.board[targetCell];
                    if (pieceOnTargetCell > 0)
                    {
                        if (Decoders.DecodeBinaryChessColour(pieceOnTargetCell) == GlobalGameVariables.ChessTurn)
                        {
                            break;
                        }
                    
                        targetPositions.Add( (new Move(startingPosition, targetCell)));
                    
                        if (Decoders.DecodeBinaryChessColour(targetCell) != GlobalGameVariables.ChessTurn)
                        {
                            break;
                        }
                    }
                    else
                    {
                        targetPositions.Add( (new Move(startingPosition, targetCell)));
                    }
                }
            }

            if (targetPositions.Count > 0)
            {
                _moves[startingPosition] = targetPositions.ToArray();
            }
        }
    }
}

