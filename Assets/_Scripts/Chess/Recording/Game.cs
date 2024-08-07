using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace Recording
{
    public class Game
    {
        public static Game Instance = new Game();
        private Dictionary<int, Turn> turns = new Dictionary<int, Turn>();
        private string _directoryName = "Saves";

        public void ClearGame()
        {
            turns = new Dictionary<int, Turn>();
        }
        
        public void AddTurn(int turnNumber, Turn turnRecord)
        {
            turns[turnNumber] = turnRecord;
        }

        public Turn ReadExactTurn(int turnNumber)
        {
            return turns[turnNumber];
        }

        public Turn ReadLastTurn()
        {
            return ReadExactTurn(Mathf.Max(turns.Keys.ToArray()));
        }

        public void SaveGame()
        {
            Debug.Log("Save started");
            SaveData data = new SaveData();
            data.FromDictionary(turns);
            data.gameEnds = GlobalGameVariables.gameStatus;
            data.dateLastTurn = DateTime.Now.ToString();

            string savePath = Path.Combine(_directoryName,GlobalGameVariables.GameId + ".json");
            
            if (File.Exists(savePath))
            {
                File.WriteAllText(savePath, String.Empty);
            }
            
            
            var file = File.CreateText(savePath);
            file.WriteLine(JsonUtility.ToJson(data));
            
            file.Close();
            Debug.Log("Save ends");
        }

        public void LoadGame()
        {
            string savePath = Path.Combine(_directoryName,GlobalGameVariables.GameId + ".json");
            
            if(File.Exists(savePath))
            {
                SaveData data =
                    JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));

                turns = data.ToDictionary();
            }
            else
            {
                Debug.LogError("Such save do not exist!");
            }
        }

        private Game()
        {
            
        }
    }

    public class SaveData
    {
        public GameEndings gameEnds;
        public string dateLastTurn;
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

    public enum GameEndings
    {
        Unfinished = 0,
        Draw = 1,
        White = 2,
        Black = 3
    }
}