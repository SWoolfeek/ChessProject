using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public readonly struct Move
    {
        public readonly int StartPosition;
        public readonly int TargetPosition;
        
        public readonly bool Castling;
        
        public Move (int startPosition, int targetPosition, bool castling = false) {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            Castling = castling;
        }
    }
}

