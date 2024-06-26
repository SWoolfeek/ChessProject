using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public static class Board
    {
        public static int[] board;
        
        private static GlobalGameVariables instance;

        public static GlobalGameVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalGameVariables();
                }

                return instance;
            }
        }

        static Board()
        {
            board = new int[64];
        }
    }
}

