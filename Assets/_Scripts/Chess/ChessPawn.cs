using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece
{
    private int _regularStep = 1;
    private int _firstStep = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        calculateMovement = CalculateMovement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateMovement()
    {
        int step = _regularStep;

        if (_firstTurn)
        {
            step = _firstStep;
        }

        positionToMove = new List<string>();
        
        for (int i = 0; i < step; i++)
        {
            positionToMove.Add(Letters[Letters.IndexOf(_position[0]) + 1 + i] + _position[1].ToString());
            Debug.Log("Calculated - " + Letters[Letters.IndexOf(_position[0]) + 1] + _position[1].ToString());
        }
    }
}
