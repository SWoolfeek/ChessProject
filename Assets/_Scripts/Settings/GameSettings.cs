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

        private string _directoryName = "Saves";
        private string _fileName = "GameSettings.json";
        private string _filePath;
        
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

            if (!Directory.Exists(_directoryName))
            {
                Directory.CreateDirectory(_directoryName);
            }
            
            if (File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, String.Empty);
            }
            
            var file = File.CreateText(_filePath);
            file.WriteLine(JsonUtility.ToJson(data));
            
            file.Close();
        }

        public void LoadSettings()
        {
            _filePath = Path.Combine(_directoryName, _fileName);
            
            if(!File.Exists(_filePath))
            {
                SaveSettings();
            }
            SavedGameSettings data =
                JsonUtility.FromJson<SavedGameSettings>(File.ReadAllText(_filePath));


            if (File.Exists(Path.Combine(_directoryName, data.PreviousGameUId + ".json")))
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

