using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceChess : MonoBehaviour
{
    [SerializeField] private BoardParameters boardParameters;

    [SerializeField] private GameObject whiteChess;
    [SerializeField] private GameObject blackChess;

    private int _pawns = 8;
    
    // Start is called before the first frame update
    public void StartToPlaceChess()
    {
        
    }

    private void SpawnController()
    {
        
    }

    private void SpawnChess(GameObject chess, Vector3 placeToSpawn, Quaternion rotation)
    {
        Instantiate(chess, placeToSpawn, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
