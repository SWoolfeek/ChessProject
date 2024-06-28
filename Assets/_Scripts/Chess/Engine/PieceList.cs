using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class PieceList
    {
        public int[] piecePositions;

        private int[] map;
        private int numPieces;

        public PieceList(int maxPieceCount = 16)
        {
            piecePositions = new int[maxPieceCount];
            map = new int[64];
            numPieces = 0;
        }

        public int Count
        {
            get
            {
                return numPieces;
            }
        }

        public void AddPiece(int position)
        {
            piecePositions[numPieces] = position;
            map[position] = numPieces;
            numPieces++;
        }
        
        public void RemovePiece(int position)
        {
            int pieceIndex = map[position];
            piecePositions[pieceIndex] = piecePositions[numPieces - 1];
            map[piecePositions[pieceIndex]] = pieceIndex;
            numPieces--;
        }

        public void MovePiece(int startingPosition, int targetPosition)
        {
            int pieceIndex = map[startingPosition];
            piecePositions[pieceIndex] = targetPosition;
            map[targetPosition] = pieceIndex;
        }
        
        public int this [int index] => piecePositions[index];
    }
}

