using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Generation/BoardParameters")]
public class BoardParameters : ScriptableObject
{
    [Min(1)] public int gridSize = 8;
    [Min(1)] public float cellSize = 2;

    public Dictionary<string, GameObject> boardCells;

    private List<string> names;
    private List<GameObject> objects;

    public void ClearData()
    {
        names = new List<string>();
        objects = new List<GameObject>();
    }

    public void AddBoardCellData(string cellName, GameObject cellGameObject)
    {
        names.Add(cellName);
        objects.Add(cellGameObject);
    }

    public void GenerateDictionary()
    {
        boardCells = new Dictionary<string, GameObject>();
        
        for (int i = 0; i < names.Count; i++)
        {
            boardCells.Add(names[i], objects[i]);
        }
    }
}
