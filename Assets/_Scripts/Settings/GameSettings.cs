using System;
using System.IO;
using UnityEngine;


namespace Settings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public bool DrawIn50Turns;
        
        public bool PreviousGameUnfinished = false;
        public string PreviousGameUId;
        
        public enum TimersType
        {
            TurnedOff = 0,
            Classic = 1,
            Fast = 2
        }

        public void SaveSettings()
        {
            SavedGameSettings data = new SavedGameSettings();

            data.PreviousGameUnfinished = PreviousGameUnfinished;
            data.PreviousGameUId = PreviousGameUId;
            
            if (File.Exists("Saves/GameSettings.json"))
            {
                File.WriteAllText("Saves/GameSettings.json", String.Empty);
            }
            
            var file = File.CreateText("Saves/GameSettings.json");
            file.WriteLine(JsonUtility.ToJson(data));
            
            file.Close();
        }

        public void LoadSettings()
        {
            if(!File.Exists("Saves/GameSettings.json"))
            {
                SaveSettings();
            }
            SavedGameSettings data =
                JsonUtility.FromJson<SavedGameSettings>(File.ReadAllText("Saves/GameSettings.json"));


            if (File.Exists("Saves/" + data.PreviousGameUId + ".json"))
            {
                PreviousGameUnfinished = data.PreviousGameUnfinished;
                PreviousGameUId = data.PreviousGameUId;
            }
            else
            {
                PreviousGameUnfinished = false;
            }
            
        }
    }

    public class SavedGameSettings
    {
        public bool PreviousGameUnfinished;
        public string PreviousGameUId;
    }
}

