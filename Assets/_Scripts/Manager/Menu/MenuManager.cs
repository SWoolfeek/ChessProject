using System;
using System.Collections;
using System.Collections.Generic;
using Recording;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Settings")] 
    
    [SerializeField] private GameSettings gameSettings;
    
    [Header("Ui")] 
    
    [SerializeField] private GameObject mainMenuWindow;
    [SerializeField] private GameObject loadWindow;
    [SerializeField] private GameObject saveContainer;
    [SerializeField] private Slider slider;
    
    [Header("Buttons")] 
    
    [SerializeField] private GameObject continueButton;

    [Header("Managers")] 
    
    [SerializeField] private LoadManager loadManager;
    
    public static MenuManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance MenuManager already exists!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        LoadSettings();
        if (gameSettings.PreviousGameUnfinished)
        {
            continueButton.SetActive(true);
        }
    }

    private void Start()
    {
        loadManager.StartLoadManager();
    }

    private void LoadSettings()
    {
        gameSettings.LoadSettings();
    }
    
    #region LoadWindow
    
    public void LoadExactGame(string gameUId)
    {
        gameSettings.PreviousGameUnfinished = true;
        gameSettings.PreviousGameUId = gameUId;
        gameSettings.SaveSettings();
        SceneManager.LoadScene("GameScene");
    }

    public void DeleteSave(string gameUId, GameObject saveElement, GameEndings gameState)
    {
        loadManager.DeleteSave(gameUId, saveElement, gameState);
    }

    public void BackToMainMenu()
    {
        loadWindow.SetActive(false);
        mainMenuWindow.SetActive(true);
    }

    public void Slider()
    {
        
    }
    
    #endregion

    #region MainMenuWindow
    
    public void ContinueGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void NewGame()
    {
        gameSettings.PreviousGameUnfinished = false;
        gameSettings.SaveSettings();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        mainMenuWindow.SetActive(false);
        loadWindow.SetActive(true);
    }

    public void Settings()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    #endregion
}
