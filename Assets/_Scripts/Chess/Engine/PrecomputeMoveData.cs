using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public static class PrecomputedMoveData
    {
        public static readonly Board BoardRepresentation = new Board();
        
        public static readonly int[] DirectionOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };
        public static readonly int[][] NumCellsToEdge;
        
        // White = 0 and Black = 1.
        public static readonly int[][] pawnAttackDirections = {
            new int[] { 4, 6 },
            new int[] { 7, 5 }
        };

        static PrecomputedMoveData()
        {
            NumCellsToEdge = new int[64][];
            
            for (int cellIndex = 0; cellIndex < 64; cellIndex++)
            {
                int row = cellIndex / 8;
                int column = cellIndex - row * 8;
                
                int north = 7 - row;
                int south = row;
                int west = column;
                int east = 7 - column;

                NumCellsToEdge[cellIndex] = new int[8];
                NumCellsToEdge[cellIndex][0] = north;
                NumCellsToEdge[cellIndex][1] = south;
                NumCellsToEdge[cellIndex][2] = west;
                NumCellsToEdge[cellIndex][3] = east;
                NumCellsToEdge[cellIndex][4] = System.Math.Min (north, west);
                NumCellsToEdge[cellIndex][5] = System.Math.Min (south, east);
                NumCellsToEdge[cellIndex][6] = System.Math.Min (north, east);
                NumCellsToEdge[cellIndex][7] = System.Math.Min (south, west);
            }
        }
    }
}

