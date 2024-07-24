using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Settings")] 
    
    [SerializeField] private GameSettings gameSettings;
    
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

    public void LoadGame(string gameUId)
    {
        gameSettings.PreviousGameUnfinished = true;
        gameSettings.PreviousGameUId = gameUId;
        gameSettings.SaveSettings();
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
