using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Chess
{
    using static PrecomputedMoveData;
    
    public class MoveGenerator
    {
        private ChessColour _chessTurn;
        
        private Dictionary<int, Move[]> _slidingMoves;
        private Dictionary<int, Move[]> _pawnMoves;
        private Dictionary<int, Move[]> _knightMoves;

        public Dictionary<int, Move[]> GenerateLegalMoves(ChessColour chessTurn)
        {
            _chessTurn = chessTurn;
            int chessTeam = _chessTurn == ChessColour.White ? 0 : 1;
            Dictionary<int, Move[]> pseudoLegalMoves = GenerateMoves(chessTeam);
            Dictionary<int, Move[]> legalMoves = new Dictionary<int, Move[]>();
            
            Debug.Log("Calculation start");
            float time = Time.time;
            // All chess in pseudoLegal moves.
            foreach (int key in pseudoLegalMoves.Keys)
            {
                
                List<Move> possibleTargets = new List<Move>();
                
                // Every move for one chess in pseudoLegal moves.
                foreach (Move verificationMove in pseudoLegalMoves[key])
                {
                    BoardRepresentation.MovePiece(verificationMove.StartPosition,verificationMove.TargetPosition);
                    Dictionary<int, Move[]> opponentResponses = GenerateMoves(1 - chessTeam);
                    
                    
                    // If it is no check for king, add this moving.
                    if (!CheckIfThisPositionUnderAttack(BoardRepresentation.kingsPosition[chessTeam], opponentResponses))
                    {
                        possibleTargets.Add(verificationMove);
                    }

                    BoardRepresentation.UndoPreviousMovement(verificationMove.StartPosition,
                        verificationMove.TargetPosition, _chessTurn);
                }
                
                if (possibleTargets.Count > 0)
                {
                    legalMoves[key] = possibleTargets.ToArray();
                }
            }

            if (legalMoves.ContainsKey(BoardRepresentation.kingsPosition[chessTeam]))
            {
                legalMoves[BoardRepresentation.kingsPosition[chessTeam]] =
                    legalMoves[BoardRepresentation.kingsPosition[chessTeam]].Concat(KingCastling(chessTeam)).ToArray();
            }
            
            time -= Time.time;
            Debug.Log("Calculation ends - " + time + " s.");
            
            return legalMoves;
        }
        
        public Dictionary<int, Move[]> GenerateMoves(int chessTeam)
        {
            return GenerateMoveForPieces(chessTeam);
        }

        private Dictionary<int, Move[]> GenerateMoveForPieces(int chessTeam)
        {
            Dictionary<int, Move[]> moves = new Dictionary<int, Move[]>();
            Move [] kingMoves = GenerateKingMoves(chessTeam);

            
            
            if (kingMoves.Length > 0)
            {
                moves[BoardRepresentation.kingsPosition[chessTeam]] = kingMoves;
            }
            
            GenerateSlidingMoves(chessTeam);
            GeneratePawnMoves(chessTeam);
            GenerateKnightMoves(chessTeam);
            
            if (_slidingMoves.Keys.Count > 0)
            {
                foreach (int slidingMoveKey in _slidingMoves.Keys)
                {
                    moves[slidingMoveKey] = _slidingMoves[slidingMoveKey];
                }
            }
            
            if (_pawnMoves.Keys.Count > 0)
            {
                foreach (int pawnMoveKey in _pawnMoves.Keys)
                {
                    moves[pawnMoveKey] = _pawnMoves[pawnMoveKey];
                }
            }
            
            if (_knightMoves.Keys.Count > 0)
            {
                foreach (int knightMoveKey in _knightMoves.Keys)
                {
                    moves[knightMoveKey] = _knightMoves[knightMoveKey];
                }
            }
            return moves;
        }

        private Move[] GenerateKingMoves(int chessTeam)
        {
            ChessColour colour = chessTeam == 0 ? ChessColour.White : ChessColour.Black;
            
            int startingPosition = BoardRepresentation.kingsPosition[chessTeam];
            
            List<Move> targetPositions = new List<Move>();
            
            for (int direction = 0; direction < 8; direction++)
            {
                if (NumCellsToEdge[startingPosition][direction] > 0)
                {
                    int targetPosition = startingPosition + DirectionOffsets[direction];
                    
                    if (BoardRepresentation.board[targetPosition] == 0 || Decoders.DecodeBinaryChessColour(BoardRepresentation.board[targetPosition]) != colour)
                    {
                        targetPositions.Add(new Move(startingPosition, targetPosition));
                    }
                }
            }
            
            return targetPositions.ToArray();
        }

        private Move[] KingCastling(int chessTeam)
        {
            ChessColour colour = chessTeam == 0 ? ChessColour.White : ChessColour.Black;
            List<Move> targetPositions = new List<Move>();
            
            if (colour == _chessTurn)
            {
                int startingPosition = BoardRepresentation.kingsPosition[chessTeam];
                
                Dictionary<int, Move[]> opponentResponses = GenerateMoves(1 - chessTeam);

                if (!CheckIfThisPositionUnderAttack(BoardRepresentation.kingsPosition[chessTeam],opponentResponses))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (BoardRepresentation.possibleCastlings[chessTeam][i])
                        {
                            int castling = GenerateCastlingMove(chessTeam, i, opponentResponses);
                            if (castling > -1)
                            {
                                targetPositions.Add(new Move(startingPosition, castling, true));
                            }
                        }
                    }
                }
            }

            return targetPositions.ToArray();
        }

        private bool CheckIfThisPositionUnderAttack(int targetPosition, Dictionary<int, Move[]> opponentResponses)
        {
            int opponentsCheck = 0;
            
            // All opponent chess which can answer for this exact move
            foreach (int opponentKey in opponentResponses.Keys)
            {
                // Check if anny opponent can capture a target.
                if (opponentResponses[opponentKey].Any(response => response.TargetPosition == targetPosition))
                {
                }
                else
                {
                    opponentsCheck++;
                }
            }

            if (opponentsCheck == opponentResponses.Keys.Count)
            {
                return false;
            }

            return true;
        }

        private int GenerateCastlingMove(int chessTeam, int castlingType, Dictionary<int, Move[]> opponentResponses)
        {
            int castlingDirection = castlingType == 0 ? 1 : -1;

            int check = 1;
            
            
            // Castling type 0 - Short, 1 - Long.
            for (int i = 1; i < 3 + castlingType; i++)
            {
                int targetPosition = (BoardRepresentation.kingsPosition[chessTeam] + i * castlingDirection);
                
                
                if (BoardRepresentation.board[targetPosition] > 0 || CheckIfThisPositionUnderAttack(targetPosition, opponentResponses))
                {
                    break;
                }
                check++;
            }

            if (check == 3 + castlingType)
            {
                return BoardRepresentation.kingsPosition[chessTeam] + (2 + castlingType) * castlingDirection;
            }

            return -99;

        }

        private void GenerateSlidingMoves(int chessTeam)
        {
            _slidingMoves = new Dictionary<int, Move[]>();
            
            for (int i = 0; i < BoardRepresentation.rooks[chessTeam].Count; i++)
            {
                GenerateSlidingPieceMoves(BoardRepresentation.rooks[chessTeam][i], 0, 4, chessTeam);
            }
            
            for (int i = 0; i < BoardRepresentation.bishops[chessTeam].Count; i++)
            {
                GenerateSlidingPieceMoves(BoardRepresentation.bishops[chessTeam][i], 4, 8, chessTeam);
            }
            
            for (int i = 0; i < BoardRepresentation.queens[chessTeam].Count; i++)
            {
                GenerateSlidingPieceMoves(BoardRepresentation.queens[chessTeam][i], 0, 8, chessTeam);
            }
        }

        private void GenerateSlidingPieceMoves(int startingPosition, int startDirIndex, int endDirIndex, int chessTeam)
        {
            ChessColour team = chessTeam == 0 ? ChessColour.White : ChessColour.Black;
            List<Move> targetPositions = new List<Move>();
            for (int directionIndex = startDirIndex; directionIndex < endDirIndex; directionIndex++)
            {
                for (int n = 0; n < NumCellsToEdge[startingPosition][directionIndex]; n++)
                {
                    int targetCell = startingPosition + DirectionOffsets[directionIndex] * (n + 1);
                    int pieceOnTargetCell = BoardRepresentation.board[targetCell];
                    if (pieceOnTargetCell > 0)
                    {
                        if (Decoders.DecodeBinaryChessColour(pieceOnTargetCell) == team)
                        {
                            break;
                        }
                    
                        targetPositions.Add( (new Move(startingPosition, targetCell)));
                    
                        if (Decoders.DecodeBinaryChessColour(pieceOnTargetCell) != team)
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
                _slidingMoves[startingPosition] = targetPositions.ToArray();
            }
        }

        private void GeneratePawnMoves(int chessTeam)
        {
            _pawnMoves = new Dictionary<int, Move[]>();
            
            int startingRow = _chessTurn == ChessColour.White ? 1 : 6;
            ChessColour team = chessTeam == 0 ? ChessColour.White : ChessColour.Black;

            for (int i = 0; i < BoardRepresentation.pawns[chessTeam].Count; i++)
            {
                List<Move> targetPositions = new List<Move>();
                
                int startingPosition = BoardRepresentation.pawns[chessTeam][i];
                int row = startingPosition / 8;
                int targetCell = startingPosition + DirectionOffsets[chessTeam];

                // Generating moves.
                if (NumCellsToEdge[startingPosition][chessTeam] > 0)
                {
                    if (BoardRepresentation.board[targetCell] == 0)
                    {
                        if (IsLastCellForTeam(chessTeam, targetCell))
                        {
                            targetPositions.Add( (new Move(startingPosition, targetCell,false,true)));
                        }
                        else
                        {
                            targetPositions.Add( (new Move(startingPosition, targetCell)));
                        }
                        
                    }

                    if (NumCellsToEdge[targetCell][chessTeam] > 0)
                    {
                        if (startingRow == row)
                        {
                            targetCell += DirectionOffsets[chessTeam];
                
                            if (BoardRepresentation.board[targetCell] == 0)
                            {
                                targetPositions.Add( (new Move(startingPosition, targetCell)));
                            }
                        }
                    }
                
                    // Generating capture.
                    for (int j = 0; j < 2; j++)
                    {
                        if (NumCellsToEdge[startingPosition][pawnAttackDirections[chessTeam][j]] > 0)
                        {
                            targetCell = startingPosition +
                                         DirectionOffsets[
                                             pawnAttackDirections[chessTeam][j]];
                            if (Decoders.DecodeBinaryChessColour(BoardRepresentation.board[targetCell]) != team && BoardRepresentation.board[targetCell] != 0)
                            {
                                if (IsLastCellForTeam(chessTeam, targetCell))
                                {
                                    targetPositions.Add( (new Move(startingPosition, targetCell,false,true)));
                                }
                                else
                                {
                                    targetPositions.Add( (new Move(startingPosition, targetCell)));
                                }
                                
                            }
                            else if (BoardRepresentation.enPassantCapturePosition > -1 && targetCell == BoardRepresentation.enPassantCapturePosition)
                            {
                                if (IsLastCellForTeam(chessTeam, targetCell))
                                {
                                    targetPositions.Add( (new Move(startingPosition, targetCell,false,true, BoardRepresentation.enPassantPawnPosition)));
                                }
                                else
                                {
                                    targetPositions.Add( (new Move(startingPosition, targetCell, false,false, BoardRepresentation.enPassantPawnPosition)));
                                }
                            }
                        }
                    }
                    
                    if (targetPositions.Count > 0)
                    {
                        _pawnMoves[startingPosition] = targetPositions.ToArray();
                    }
                }
                
            }
            
        }

        private bool IsLastCellForTeam(int chessTeam, int targetCell)
        {
            if (chessTeam == 0)
            {
                if (targetCell > 55)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (targetCell < 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void GenerateKnightMoves(int chessTeam)
        {

            _knightMoves = new Dictionary<int, Move[]>();
            
            for (int i = 0; i < BoardRepresentation.knights[chessTeam].Count; i++)
            {
                List<Move> targetPositions = new List<Move>();
                int startingPosition = BoardRepresentation.knights[chessTeam][i];

                for (int j = 0; j < 4; j++)
                {
                    if (NumCellsToEdge[startingPosition][j] > 1)
                    {
                        targetPositions.AddRange(CalculateKnightMove(startingPosition, j, chessTeam));
                    }
                }
                
                if (targetPositions.Count > 0)
                {
                    _knightMoves[startingPosition] = targetPositions.ToArray();
                }
            }
        }

        private List<Move> CalculateKnightMove(int startingPosition, int direction, int chessTeam)
        {
            List<Move> result = new List<Move>();

            for (int i = 0; i < 2; i++)
            {
                if (NumCellsToEdge[startingPosition][knightDirections[direction][i]] > 0)
                {
                    int targetPosition = startingPosition + knightMoves[direction * 2 + i];
                    
                    if (BoardRepresentation.board[targetPosition] == 0 || Decoders.DecodeBinaryChessColour(BoardRepresentation.board[targetPosition]) != (ChessColour)chessTeam)
                    {
                        result.Add(new Move(startingPosition, targetPosition));
                    }
                }
            }

            return result;
        }
    }
}

