using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Recording;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewatchManager : MonoBehaviour
{
    [Header("Ui")] 
    
    [SerializeField] private Slider slider;
    [SerializeField] private float autoplayDelay;

    [Header("Board")] 
    
    [SerializeField] private ChessGenerationRewatch chessGenerationManager;
    [SerializeField] private GenerateBoardPrefab boardGenerator;

    [Header("Settings")] 
    
    [SerializeField] private GameSettings gameSettings;
    
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
        if(File.Exists("Saves/" + GlobalGameVariables.GameId + ".json"))
        {
            _loadedGame =
                JsonUtility.FromJson<SaveData>(File.ReadAllText("Saves/" + GlobalGameVariables.GameId + ".json"));
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

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ContinueFromThisTurn()
    {
        _autoPlay = false;
        
        if ((int)slider.maxValue != (int)slider.value || _loadedGame.gameEnds == GameEndings.Unfinished)
        {
            CreateNewSave((int)slider.value);
        }
    }

    private void CreateNewSave(int lastTurn)
    {
        Game.Instance.ClearGame();
        
        for (int i = 1; i < lastTurn + 1; i++)
        {
            Game.Instance.AddTurn(i,_turns[i]);
        }

        GenerateRandomId idGenerator = new GenerateRandomId();
        GlobalGameVariables.GameId = idGenerator.Generate();
        GlobalGameVariables.gameStatus = GameEndings.Unfinished;
        Game.Instance.SaveGame();

        gameSettings.PreviousGameUId = GlobalGameVariables.GameId;
        gameSettings.PreviousGameUnfinished = true;
        gameSettings.SaveSettings();

        SceneManager.LoadScene("GameScene");
    }

    public void GetFEN()
    {
        _autoPlay = false;
        
        TextEditor te = new TextEditor();
        te.content = new GUIContent(_turns[(int)slider.value].FEN);
        te.SelectAll();
        te.Copy();
    }

    #region RecordControls
    
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
    
    #endregion  
}
