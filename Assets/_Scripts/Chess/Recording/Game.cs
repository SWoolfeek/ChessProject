using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Recording
{
    public class Game
    {
        public static Game Instance = new Game();
        private Dictionary<int, Turn> turns = new Dictionary<int, Turn>();

        public void AddTurn(int turnNumber, Turn turnRecord)
        {
            turns[turnNumber] = turnRecord;
        }

        public Turn ReadExactTurn(int turnNumber)
        {
            return turns[turnNumber];
        }

        private Game()
        {
            
        }
    }
}