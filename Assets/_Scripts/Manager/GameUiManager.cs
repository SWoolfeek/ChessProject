using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUiManager : MonoBehaviour
{
    [Header("Camera")] 
    
    [SerializeField] private GameObject uiCamera;
    
    [Header("Ui")] 
    
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject pauseScreen;
    
    [SerializeField] private GameObject whitePromotionScreen;
    [SerializeField] private GameObject blackPromotionScreen;
    
    [Header("Settings")] 
    
    [SerializeField] private GameSettings gameSettings;

    private bool _pauseMenuActive;

    #region PauseMenu

    public void PauseMenu()
    {
        _pauseMenuActive = !_pauseMenuActive;
        pauseScreen.SetActive(_pauseMenuActive);
    }

    #endregion
    
    #region GameEnd

    public void GameFinished()
    {
        loseScreen.SetActive(true);
    }

    public void RestartTheGame()
    {
        gameSettings.PreviousGameUnfinished = false;
        gameSettings.SaveSettings();
        SceneManager.LoadScene("GameScene");
    }

    public void ReWatchTheGame()
    {
        SceneManager.LoadScene("RewatchGameScene");
    }

    #endregion

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    
    #region Promotion

    public void Promotion(ChessColour colour)
    {
        uiCamera.SetActive(true);
        
        if (colour == ChessColour.White)
        {
            whitePromotionScreen.SetActive(true);
        }
        else
        {
            blackPromotionScreen.SetActive(true);
        }
    }
    
    public void QueenPromotion()
    {
        PromotionEnd();
        GameManager.Instance.PromotionResult(ChessType.Queen);
    }
    
    public void RookPromotion()
    {
        PromotionEnd();
        GameManager.Instance.PromotionResult(ChessType.Rook);
    }
    
    public void BishopPromotion()
    {
        PromotionEnd();
        GameManager.Instance.PromotionResult(ChessType.Bishop);
    }
    
    public void KnightPromotion()
    {
        PromotionEnd();
        GameManager.Instance.PromotionResult(ChessType.Knight);
    }

    private void PromotionEnd()
    {
        uiCamera.SetActive(false);
        whitePromotionScreen.SetActive(false);
        blackPromotionScreen.SetActive(false);
    }
    
    #endregion
    
}
