using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public void SaveGame()
        {
            SaveData data = new SaveData();
            data.FromDictionary(turns);
            
            if (File.Exists("Saves/" + GlobalGameVariables.GameId + ".json"))
            {
                File.Delete("Saves/" + GlobalGameVariables.GameId + ".json");
            }
            
            var file = File.CreateText("Saves/" + GlobalGameVariables.GameId + ".json");
            file.WriteLine(JsonUtility.ToJson(data));
            
            file.Close();
        }

        private Game()
        {
            
        }
    }

    public class SaveData
    {
        public int turnsAmount;
        public List<int> keys;
        public List<string> values;

        public void FromDictionary(Dictionary<int, Turn> turns)
        {
            keys = new List<int>();
            values = new List<string>();
            turnsAmount = 0;
            
            foreach (int key in turns.Keys)
            {
                turnsAmount++;
                keys.Add(key);
                values.Add(JsonUtility.ToJson(turns[key]));
            }
        }

        public Dictionary<int, Turn> ToDictionary()
        {
            Dictionary<int, Turn> result = new Dictionary<int, Turn>();
            for (int i = 0; i < keys.Count; i++)
            {
                result[keys[i]] = JsonUtility.FromJson<Turn>(values[i]);
            }

            return result;
        }
        
    }
}