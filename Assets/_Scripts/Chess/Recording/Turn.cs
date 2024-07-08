using System.Collections;
using System.Collections.Generic;
using Chess;
using UnityEngine;

namespace Recording
{
    public class Turn
    {
        private readonly Dictionary<int, char> IntToFen = new Dictionary<int, char> 
        {
            { 9, 'K' }, 
            { 10, 'P' },
            { 11, 'N' },
            { 12, 'B' },
            { 13, 'R' },
            { 14, 'Q' },
            { 17, 'k' }, 
            { 18, 'p' },
            { 19, 'n' },
            { 20, 'b' },
            { 21, 'r' },
            { 22, 'q' }
        };
        
        public readonly string FEN;

        public Turn()
        {
            FEN = ConvertBoardToFEN();
        }

        private string ConvertBoardToFEN()
        {
            int[] board = PrecomputedMoveData.BoardRepresentation.board;

            List<string> fen = new List<string>();
            string row = "";
            int toSkip = 0;

            for (int i = 0; i < 64; i++)
            {
                if (board[i] == 0)
                {
                    toSkip++;
                }
                else
                {
                    if (toSkip > 0)
                    {
                        row += toSkip;
                        toSkip = 0; 
                    }

                    row += IntToFen[board[i]];
                }
                
                if ((i + 1) %8 == 0)
                {
                    Debug.Log("i = " + i);
                    if (toSkip > 0)
                    {
                        row += toSkip;
                    }
                    
                    fen.Add(row);
                    row = "";
                    toSkip = 0;
                }
                
                
            }

            string result = "";
            fen.Reverse();

            for (int i = 0; i < fen.Count; i++)
            {
                result += fen[i];
                
                if (i != fen.Count - 1)
                {
                    result += '/';
                }
            }

            return result;
        }
    }
}

