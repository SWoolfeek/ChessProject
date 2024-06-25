using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChessPosition
{
    private Dictionary<string, ChessType> _chessPositions = new Dictionary<string, ChessType>();

    public Dictionary<string, ChessType> BasicWhitePositions()
    {
        _chessPositions.Clear();

        // Pawns.
        _chessPositions["A2"] = ChessType.Pawn;
        _chessPositions["B2"] = ChessType.Pawn;
        _chessPositions["C2"] = ChessType.Pawn;
        _chessPositions["D2"] = ChessType.Pawn;
        _chessPositions["E2"] = ChessType.Pawn;
        _chessPositions["F2"] = ChessType.Pawn;
        _chessPositions["G2"] = ChessType.Pawn;
        _chessPositions["H2"] = ChessType.Pawn;
        
        // Rooks.
        _chessPositions["A1"] = ChessType.Rook;
        _chessPositions["H1"] = ChessType.Rook;
        
        // Knights.
        _chessPositions["B1"] = ChessType.Knight;
        _chessPositions["G1"] = ChessType.Knight;
        
        // Bishops.
        _chessPositions["C1"] = ChessType.Bishop;
        _chessPositions["F1"] = ChessType.Bishop;
        
        // Queen.
        _chessPositions["D1"] = ChessType.Queen;
        
        // King.
        _chessPositions["E1"] = ChessType.King;
        
        return _chessPositions;
    }
    
    public Dictionary<string, ChessType> BasicBlackPositions()
    {
        _chessPositions.Clear();

        // Pawns.
        _chessPositions["A7"] = ChessType.Pawn;
        _chessPositions["B7"] = ChessType.Pawn;
        _chessPositions["C7"] = ChessType.Pawn;
        _chessPositions["D7"] = ChessType.Pawn;
        _chessPositions["E7"] = ChessType.Pawn;
        _chessPositions["F7"] = ChessType.Pawn;
        _chessPositions["G7"] = ChessType.Pawn;
        _chessPositions["H7"] = ChessType.Pawn;
        
        // Rooks.
        _chessPositions["A8"] = ChessType.Rook;
        _chessPositions["H8"] = ChessType.Rook;
        
        // Knights.
        _chessPositions["B8"] = ChessType.Knight;
        _chessPositions["G8"] = ChessType.Knight;
        
        // Bishops.
        _chessPositions["C8"] = ChessType.Bishop;
        _chessPositions["F8"] = ChessType.Bishop;
        
        // Queen.
        _chessPositions["D8"] = ChessType.Queen;
        
        // King.
        _chessPositions["E8"] = ChessType.King;
        
        return _chessPositions;
    }
}
