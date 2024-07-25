using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Recording;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    [Header("Ui elements")]
    
    [SerializeField] private GameObject loadElementsParent;

    [Header("Prefabs")] 
    
    [SerializeField] private GameObject loadGamePrefab;
    
    private Dictionary<string, LoadData> _loadDatas = new Dictionary<string, LoadData>();
    private List<DateTime> _dataKeys = new List<DateTime>();
    
    // Start is called before the first frame update
    public void StartLoadManager()
    {
        LoadAllSaves();
        CreateLoadUIElements();
    }

    private void CreateLoadUIElements()
    {
        foreach (DateTime key in _dataKeys)
        {
            LoadData data = _loadDatas[key.ToString()];
            Instantiate(loadGamePrefab, loadElementsParent.transform).GetComponent<LoadElement>().SetData(data.gameEnds, key.ToString(), data.UId);
        }
    }

    private void LoadAllSaves()
    {
        foreach (string file in Directory.GetFiles("Saves", "*.json"))
        {
            string fileName = file.Split("\\")[1];
            if (fileName != "GameSettings.json")
            {
                LoadSave(fileName);
            }
        }
        
        _dataKeys.Sort((x, y) => y.CompareTo(x));
    }

    private void LoadSave(string fileName)
    {
        if(File.Exists("Saves/" + fileName))
        {
            SaveData data =
                JsonUtility.FromJson<SaveData>(File.ReadAllText("Saves/" + fileName));
            
            _dataKeys.Add(DateTime.Parse(data.dateLastTurn));

            LoadData loadData = new LoadData(fileName.Split(".json")[0], data.gameEnds, data.turnsAmount);
            _loadDatas[data.dateLastTurn] = loadData;
        }
    }

    public class LoadData
    {
        public string UId;
        public GameEndings gameEnds;
        public int turns;

        public LoadData(string inputUId, GameEndings inputGameEnds, int inputTurns)
        {
            UId = inputUId;
            gameEnds = inputGameEnds;
            turns = inputTurns;
        }
    }
}


