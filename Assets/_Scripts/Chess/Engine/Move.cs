using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public readonly struct Move
    {
        public readonly int StartPosition;
        public readonly int TargetPosition;
        public readonly int EnPassantCapture;
        
        public readonly bool Castling;
        
        public Move (int startPosition, int targetPosition, bool castling = false, int enPassantCapture = -99) {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            EnPassantCapture = enPassantCapture;
            Castling = castling;
        }
    }
}

