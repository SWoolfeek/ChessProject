using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Recording;
using UnityEngine;
using UnityEngine.UI;

public class RewatchManager : MonoBehaviour
{
    // Delete after testing.
    public string loadGameForTesting;
    [Header("Ui")] 
    
    [SerializeField] private Slider slider;
    [SerializeField] private float autoplayDelay;

    [Header("Board")] 
    
    [SerializeField] private ChessGenerationRewatch chessGenerationManager;
    [SerializeField] private GenerateBoardPrefab boardGenerator;
    
    private SaveData _loadedGame;
    private Dictionary<string, GameObject> _cells;
    private Dictionary<int, Turn> _turns;
    private List<int> keys;
    private bool _autoPlay;
    
    public static RewatchManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance RewatchManager already exists!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        _cells = boardGenerator.ReadExistedCells();
        
        chessGenerationManager.SetBoard(_cells);
        LoadGame();
        slider.maxValue = keys.Count;
        slider.onValueChanged.AddListener (delegate {SliderValueChangeCheck ();});
    }

    private void LoadGame()
    {
        if(File.Exists("Saves/" + loadGameForTesting + ".json"))
        {
            _loadedGame =
                JsonUtility.FromJson<SaveData>(File.ReadAllText("Saves/" + loadGameForTesting + ".json"));
            _turns = _loadedGame.ToDictionary();
            Debug.Log(_turns[1].FEN);
            chessGenerationManager.StartChessGenerationRewatch(_turns[1].FEN);
            keys = _turns.Keys.ToList();
            keys.Sort((x, y) => x.CompareTo(y));
        }
    }
    

    private void SliderValueChangeCheck()
    {
        LoadTurn((int)slider.value);
    }

    private void LoadTurn(int turn)
    {
        chessGenerationManager.LoadTurn(_turns[turn].FEN);
    }

    public void NextTurn()
    {
        slider.value++;
        _autoPlay = false;
    }
    
    public void PreviousTurn()
    {
        slider.value--;
        _autoPlay = false;
    }

    public void SliderDrag()
    {
        _autoPlay = false;
    }

    public void AutoPlay()
    {
        _autoPlay = !_autoPlay;
        if (_autoPlay)
        {
            StartCoroutine(AutoPlayTimer(autoplayDelay));
        }
    }

    IEnumerator AutoPlayTimer(float delay)
    {
        while (slider.value < slider.maxValue && _autoPlay)
        {
            slider.value++;
            yield return new WaitForSeconds(delay);
        }

        _autoPlay = false;
    }
}
