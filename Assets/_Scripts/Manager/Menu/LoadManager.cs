using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Recording;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [Header("Ui elements")]
    
    
    [SerializeField] private GameObject loadElementsParent;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField][Min(0)] private int loadElementHeight;
    [SerializeField] private VerticalLayoutGroup saveContainer;
    [SerializeField] private Slider slider;

    [Header("Prefabs")] 
    
    [SerializeField] private GameObject loadGamePrefab;
    
    private Dictionary<string, LoadData> _loadDatas = new Dictionary<string, LoadData>();
    private Dictionary<GameEndings, List<GameObject>> _loadElements = new Dictionary<GameEndings, List<GameObject>>()
    {
        {GameEndings.Unfinished, new List<GameObject>()},
        {GameEndings.Draw, new List<GameObject>()},
        {GameEndings.White, new List<GameObject>()},
        {GameEndings.Black, new List<GameObject>()},
    };
    private List<DateTime> _dataKeys = new List<DateTime>();

    private float _activeLoadElements;
    private Transform saveContainerTransform;
    private RectTransform saveContainerRectTransform;
    
    // Start is called before the first frame update
    public void StartLoadManager()
    {
        saveContainerTransform = saveContainer.GetComponent<Transform>();
        saveContainerRectTransform = saveContainer.GetComponent<RectTransform>();
        
        LoadAllSaves();
        CreateLoadUIElements();
    }

    private void CreateLoadUIElements()
    {
        foreach (DateTime key in _dataKeys)
        {
            LoadData data = _loadDatas[key.ToString()];
            GameObject loadElement = Instantiate(loadGamePrefab, loadElementsParent.transform);
            loadElement.GetComponent<LoadElement>().SetData(data.gameEnds, key.ToString(), data.UId);
            _loadElements[data.gameEnds].Add(loadElement);
        }

        ReCalculateSizeLoadElements();
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

    public void FilterLoaded(int input)
    {
        Debug.Log(input);
        switch (dropdown.value)
        {
            case 0:
                foreach (GameEndings key in _loadElements.Keys)
                {
                    foreach (GameObject loadElement in _loadElements[key])
                    {
                        loadElement.SetActive(true);
                    }
                }
                ReCalculateSizeLoadElements();
                break;
            case 1:
                Filter(GameEndings.Unfinished);
                break;
            case 2:
                Filter(GameEndings.Draw);
                break;
            case 3:
                Filter(GameEndings.White);
                break;
            case 4:
                Filter(GameEndings.Black);
                break;
        }
    }

    private void Filter(GameEndings gameStatus)
    {
        foreach (GameEndings key in _loadElements.Keys)
        {
            if (key == gameStatus)
            {
                foreach (GameObject loadElement in _loadElements[key])
                {
                    loadElement.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject loadElement in _loadElements[key])
                {
                    loadElement.SetActive(false);
                }
            }
        }

        ReCalculateSizeLoadElements();
    }
    
    public void Slider()
    {
        saveContainer.padding.top = (int)(-_activeLoadElements * slider.value);
        LayoutRebuilder.ForceRebuildLayoutImmediate(saveContainerRectTransform);
    }

    private void ReCalculateSizeLoadElements()
    {
        int childs = 0;
        foreach(Transform child in saveContainerTransform){
            if(child.gameObject.activeSelf)
                childs++;
        }

        float loadElementsSize = loadElementHeight + saveContainer.spacing;

        _activeLoadElements = childs * loadElementsSize - loadElementsSize;
        Slider();
        slider.value = 0;
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


