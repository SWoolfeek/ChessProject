using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Chess
{
    using static PrecomputedMoveData;
    
    public class MoveGenerator
    {
        
        private Dictionary<int, Move[]> _moves;

        public Dictionary<int, Move[]> GenerateLegalMoves()
        {
            int chessTeam = GlobalGameVariables.ChessTurn == ChessColour.White ? 0 : 1;
            Dictionary<int, Move[]> pseudoLegalMoves = GenerateMoves(chessTeam);
            Dictionary<int, Move[]> legalMoves = new Dictionary<int, Move[]>();

            Debug.Log("Starting calculation");
            float time = Time.time;
            
            Debug.Log(BoardRepresentation.kingsPosition[chessTeam]);
            
            foreach (int key in pseudoLegalMoves.Keys)
            {
                List<Move> possibleTargets = new List<Move>();
                
                foreach (Move verificationMove in pseudoLegalMoves[key])
                {
                    
                    BoardRepresentation.MovePiece(verificationMove.StartPosition,verificationMove.TargetPosition);
                    Dictionary<int, Move[]> opponentResponses = GenerateMoves(1 - chessTeam);

                    foreach (int  opponentKey in opponentResponses.Keys)
                    {
                        int check = 0;

                        foreach (Move moving in opponentResponses[opponentKey])
                        {

                            if (moving.StartPosition == 25)
                            {
                                Debug.Log(moving.TargetPosition);
                            }
                            
                            if (moving.TargetPosition == BoardRepresentation.kingsPosition[chessTeam])
                            {
                                Debug.Log("No please!");
                                break;
                            }
                            check++;
                        }

                        if (opponentResponses[opponentKey].Length == check)
                        {
                            possibleTargets.Add(verificationMove);
                        }
                        
                        /*Debug.Log(opponentKey);
                        if (opponentResponses[opponentKey].Any(response => response.TargetPosition == BoardRepresentation.kingsPosition[chessTeam]))
                        {
                            Debug.Log("No please!");
                        }
                        else
                        {
                            possibleTargets.Add(verificationMove);
                        }*/
                    }

                    BoardRepresentation.UndoPreviousMovement(verificationMove.StartPosition,
                        verificationMove.TargetPosition, GlobalGameVariables.ChessTurn);

                }
                
                if (possibleTargets.Count > 0)
                {
                    legalMoves[key] = possibleTargets.ToArray();
                }
            }
            
            time -= Time.time;
            Debug.Log("Calculation ends - " + time + " s.");

            return legalMoves;
        }
        
        public Dictionary<int, Move[]> GenerateMoves(int chessTeam)
        {
            _moves = new Dictionary<int, Move[]>();
            
            for (int startPosition = 0; startPosition < 64; startPosition++)
            {
                int piece= BoardRepresentation.board[startPosition];
                if (Decoders.DecodeBinaryChessColour(piece) == GlobalGameVariables.ChessTurn)
                {
                    GenerateMoveForPieces(chessTeam);
                }
            }

            

            return _moves;
        }

        private void GenerateMoveForPieces(int chessTeam)
        {
            GenerateKingMoves(chessTeam);
            GenerateSlidingMoves(chessTeam);
            GeneratePawnMoves(chessTeam);
            GenerateKnightMoves(chessTeam);
        }

        private void GenerateKingMoves(int chessTeam)
        {
            int startingPosition = BoardRepresentation.kingsPosition[chessTeam];
            
            List<Move> targetPositions = new List<Move>();
            
            for (int direction = 0; direction < 8; direction++)
            {
                if (NumCellsToEdge[startingPosition][direction] > 0)
                {
                    int targetPosition = startingPosition + DirectionOffsets[direction];
                    
                    if (BoardRepresentation.board[targetPosition] == 0 || Decoders.DecodeBinaryChessColour(BoardRepresentation.board[targetPosition]) != GlobalGameVariables.ChessTurn)
                    {
                        targetPositions.Add(new Move(startingPosition, targetPosition));
                    }
                }
            }
            
            if (targetPositions.Count > 0)
            {
                _moves[startingPosition] = targetPositions.ToArray();
            }
        }

        private void GenerateSlidingMoves(int chessTeam)
        {
            
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
                _moves[startingPosition] = targetPositions.ToArray();
            }
        }

        private void GeneratePawnMoves(int chessTeam)
        {
            int startingRow = GlobalGameVariables.ChessTurn == ChessColour.White ? 1 : 6;
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
                        targetPositions.Add( (new Move(startingPosition, targetCell)));
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

        private void GenerateKnightMoves(int chessTeam)
        {

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
                    _moves[startingPosition] = targetPositions.ToArray();
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

