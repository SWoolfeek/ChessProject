using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGenerationManagerGame : ChessGenerationManager
{
    public static ChessGenerationManagerGame Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance ChessGenerationManagerGame already exists!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
