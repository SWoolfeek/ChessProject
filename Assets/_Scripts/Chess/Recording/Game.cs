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
        public List<int> _keys;
        public List<string> _values;

        public void FromDictionary(Dictionary<int, Turn> turns)
        {
            _keys = new List<int>();
            _values = new List<string>();
            
            foreach (int key in turns.Keys)
            {
                _keys.Add(key);
                _values.Add(JsonUtility.ToJson(turns[key]));
            }
        }

        public Dictionary<int, Turn> ToDictionary()
        {
            Dictionary<int, Turn> result = new Dictionary<int, Turn>();
            for (int i = 0; i < _keys.Count; i++)
            {
                result[_keys[i]] = JsonUtility.FromJson<Turn>(_values[i]);
            }

            return result;
        }
        
    }
}