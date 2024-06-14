using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private GenerateBoard board;
    
    // Start is called before the first frame update
    void Start()
    {
        board.StartBoardGeneration();
    }
}
