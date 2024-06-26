using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public readonly struct Move
    {
        public readonly int StartPosition;
        public readonly int TargetPosition;
        
        public Move (int startPosition, int targetPosition) {
            StartPosition = startPosition;
            TargetPosition = targetPosition;
        }
    }
}

