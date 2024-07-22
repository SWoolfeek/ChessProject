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
    
    private void Awake()
    {
        LoadSettings();
        if (gameSettings.PreviousGameUnfinished)
        {
            continueButton.SetActive(true);
        }
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

    public void LoadGame()
    {
        
    }

    public void Settings()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
