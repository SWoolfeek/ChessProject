using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chess
{
    using static PrecomputedMoveData;
    
    public class MoveGenerator
    {
        private Dictionary<int, Move[]> _moves;
        
        public Dictionary<int, Move[]> GenerateMoves()
        {
            _moves = new Dictionary<int, Move[]>();
            
            for (int startPosition = 0; startPosition < 64; startPosition++)
            {
                int piece= BoardRepresentation.board[startPosition];
                if (Decoders.DecodeBinaryChessColour(piece) == GlobalGameVariables.ChessTurn)
                {
                    GenerateSlidingMoves();
                    GeneratePawnMoves();
                    GenerateKnightMoves();
                }
            }

            return _moves;
        }

        private void GenerateSlidingMoves()
        {
            int chessTeam = GlobalGameVariables.ChessTurn == ChessColour.White ? 0 : 1;
            
            for (int i = 0; i < BoardRepresentation.rooks[chessTeam].Count; i++)
            {
                GenerateSlidingPieceMoves(BoardRepresentation.rooks[chessTeam][i], 0, 4);
            }
            
            for (int i = 0; i < BoardRepresentation.bishops[chessTeam].Count; i++)
            {
                GenerateSlidingPieceMoves(BoardRepresentation.bishops[chessTeam][i], 4, 8);
            }
            
            for (int i = 0; i < BoardRepresentation.queens[chessTeam].Count; i++)
            {
                GenerateSlidingPieceMoves(BoardRepresentation.queens[chessTeam][i], 0, 8);
            }
        }

        private void GenerateSlidingPieceMoves(int startingPosition, int startDirIndex, int endDirIndex)
        {
            
            List<Move> targetPositions = new List<Move>();
            for (int directionIndex = startDirIndex; directionIndex < endDirIndex; directionIndex++)
            {
                for (int n = 0; n < NumCellsToEdge[startingPosition][directionIndex]; n++)
                {
                    int targetCell = startingPosition + DirectionOffsets[directionIndex] * (n + 1);
                    int pieceOnTargetCell = BoardRepresentation.board[targetCell];
                    if (pieceOnTargetCell > 0)
                    {
                        if (Decoders.DecodeBinaryChessColour(pieceOnTargetCell) == GlobalGameVariables.ChessTurn)
                        {
                            break;
                        }
                    
                        targetPositions.Add( (new Move(startingPosition, targetCell)));
                    
                        if (Decoders.DecodeBinaryChessColour(pieceOnTargetCell) != GlobalGameVariables.ChessTurn)
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

        private void GeneratePawnMoves()
        {
            int directionIndex = GlobalGameVariables.ChessTurn == ChessColour.White ? 0 : 1;
            int startingRow = GlobalGameVariables.ChessTurn == ChessColour.White ? 1 : 6;

            for (int i = 0; i < BoardRepresentation.pawns[directionIndex].Count; i++)
            {
                List<Move> targetPositions = new List<Move>();
                
                int startingPosition = BoardRepresentation.pawns[directionIndex][i];
                int row = startingPosition / 8;
                int targetCell = startingPosition + DirectionOffsets[directionIndex];

                // Generating moves.
                if (NumCellsToEdge[startingPosition][directionIndex] > 0)
                {
                    if (BoardRepresentation.board[targetCell] == 0)
                    {
                        targetPositions.Add( (new Move(startingPosition, targetCell)));
                    }

                    if (NumCellsToEdge[targetCell][directionIndex] > 0)
                    {
                        if (startingRow == row)
                        {
                            targetCell += DirectionOffsets[directionIndex];
                
                            if (BoardRepresentation.board[targetCell] == 0)
                            {
                                targetPositions.Add( (new Move(startingPosition, targetCell)));
                            }
                        }
                    }
                
                    // Generating capture.
                    for (int j = 0; j < 2; j++)
                    {
                        if (NumCellsToEdge[startingPosition][pawnAttackDirections[directionIndex][j]] > 0)
                        {
                            targetCell = startingPosition +
                                         DirectionOffsets[
                                             pawnAttackDirections[directionIndex][j]];
                            if (Decoders.DecodeBinaryChessColour(BoardRepresentation.board[targetCell]) != GlobalGameVariables.ChessTurn)
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

        private void GenerateKnightMoves()
        {
            int chessTeam = GlobalGameVariables.ChessTurn == ChessColour.White ? 0 : 1;

            for (int i = 0; i < BoardRepresentation.knights[chessTeam].Count; i++)
            {
                List<Move> targetPositions = new List<Move>();
                int startingPosition = BoardRepresentation.knights[chessTeam][i];

                for (int j = 0; j < 4; j++)
                {
                    if (NumCellsToEdge[startingPosition][j] > 1)
                    {
                        targetPositions.AddRange(CalculateKnightMove(startingPosition, j));
                    }
                }
                
                if (targetPositions.Count > 0)
                {
                    _moves[startingPosition] = targetPositions.ToArray();
                }
            }
        }

        private List<Move> CalculateKnightMove(int startingPosition, int direction)
        {
            List<Move> result = new List<Move>();

            for (int i = 0; i < 2; i++)
            {
                if (NumCellsToEdge[startingPosition][knightDirections[direction][i]] > 0)
                {
                    int targetPosition = startingPosition + knightMoves[direction * 2 + i];
                    
                    if (BoardRepresentation.board[targetPosition] == 0 || Decoders.DecodeBinaryChessColour(BoardRepresentation.board[targetPosition]) != GlobalGameVariables.ChessTurn)
                    {
                        result.Add(new Move(startingPosition, targetPosition));
                    }
                }
            }

            return result;
        }
    }
}

