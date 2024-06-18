using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GenerateBoardPrefab : MonoBehaviour
{
    
#if UNITY_EDITOR
    [SerializeField] public BoardParameters parameters;

    [SerializeField] public List<GameObject> cellPrefabs;
    

    private bool _blackCellToInstantiate = true;
    const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Start is called before the first frame update
    public void StartGeneration()
    {
        DeleteAllCells();
    }

    private void DeleteAllCells()
    {
        
        int counter = 0;
        Transform[] childrens = transform.GetComponentsInChildren<Transform>(true);
        for (int i = 1; i < childrens.Length; i++)
        {
            counter++;
            DestroyImmediate(childrens[i].gameObject, true);
        }
        Debug.Log("Deleted - " + counter + " objects");
        
        Generate();
    }

    private void Generate()
    {
        int row = 0;
        int column = 0;
        int prefabIndex = 0;
        
        
        for (int i = 0; i < (parameters.gridSize*parameters.gridSize); i++)
        {
            row = i / parameters.gridSize;
            column = i % parameters.gridSize;
            
            if ((i + row) % 2 == 0)
            {
                prefabIndex = 0;
            }
            else
            {
                prefabIndex = 1;
            }

            Vector3 positionToSpawn = new Vector3(column * parameters.cellSize, 0 , row * parameters.cellSize);
            GameObject cell = PrefabUtility.InstantiatePrefab(cellPrefabs[prefabIndex]) as GameObject;
            cell.transform.position = positionToSpawn;
            cell.transform.parent = transform;
            cell.name = Letters[row].ToString() + (column + 1).ToString();
        }
    }
#endif


    public Dictionary<string, GameObject> ReadExistedCells()
    {
        Dictionary<string, GameObject> result = new Dictionary<string, GameObject>();
        
        Transform[] childrens = transform.GetComponentsInChildren<Transform>(true);
        for (int i = 1; i < childrens.Length; i++)
        {
            result.Add(childrens[i].name, childrens[i].gameObject);
        }

        return result;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GenerateBoardPrefab))]
public class GenerateBoardPrefabEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        GenerateBoardPrefab boardGen = (GenerateBoardPrefab)target;
        SerializedObject so = new SerializedObject(boardGen);

        SerializedProperty parametersProperty = so.FindProperty("parameters");
        SerializedProperty prefabListProperty = so.FindProperty("cellPrefabs");

        EditorGUILayout.PropertyField(parametersProperty, true);
        EditorGUILayout.PropertyField(prefabListProperty, true);
        so.ApplyModifiedProperties();
        
        
        if(GUILayout.Button("Generate"))
            boardGen.StartGeneration();
    }
}
#endif
