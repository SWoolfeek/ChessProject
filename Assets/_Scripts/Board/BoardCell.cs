using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    public GameObject chess;
    
    

    private void OnMouseDown()
    {
        Debug.Log(name);
        chess.transform.position += Vector3.up;
    }
}
