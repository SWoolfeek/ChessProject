using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour
{
    [SerializeField] private BoardParameters boardParameters;

    [SerializeField] private Material material;
    
    readonly Vector3 _startingPoint = Vector3.zero;
    
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _tris = new List<int>();
    private List<Vector3> _normals = new List<Vector3>();
    private Vector2[] _uv;
    
    // Start is called before the first frame update
    public void StartBoardGeneration()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        
        Mesh mesh = new Mesh();
        
        GenerateVertices();
        GenerateTris();
        GenerateNormals();
        GenerateUv();

        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _tris.ToArray();
        mesh.normals = _normals.ToArray();
        mesh.uv = _uv;
        
        meshFilter.mesh = mesh;
    }

    private void GenerateVertices()
    {
        int verticesAmount = (boardParameters.gridSize + 1)*(boardParameters.gridSize + 1);
        
        for (int i = 0; i < verticesAmount; i++)
        {
            int row = i / (boardParameters.gridSize + 1);
            int column = i % (boardParameters.gridSize + 1);
            _vertices.Add(_startingPoint + Vector3.forward * row * boardParameters.cellSize + Vector3.right * column * boardParameters.cellSize);
        }
        
        Debug.Log("Vertices - " + _vertices);
    }

    private void GenerateTris()
    {
        for (int i = 0; i < (boardParameters.gridSize * boardParameters.gridSize); i++)
        {
            int row = i / (boardParameters.gridSize);
            int column = i % (boardParameters.gridSize);
            
            //First tris
            _tris.Add(0 + column + row * (boardParameters.gridSize + 1));
            _tris.Add(boardParameters.gridSize + column + 1 + row * (boardParameters.gridSize + 1));
            _tris.Add(1 + column + row * (boardParameters.gridSize + 1));
            
            
            //Second tris
            _tris.Add(boardParameters.gridSize + column + 1 + row * (boardParameters.gridSize + 1));
            _tris.Add(boardParameters.gridSize + column + 2 + row * (boardParameters.gridSize + 1));
            _tris.Add(1 + column + row * (boardParameters.gridSize + 1));
            
            print("Row - " + row + " Column - " + column);
        }
    }

    private void GenerateNormals()
    {
        for (int i = 0; i < (boardParameters.gridSize + 1)*(boardParameters.gridSize + 1); i++)
        {
            _normals.Add(-Vector3.up);
        }
    }

    private void GenerateUv()
    {
        _uv = new Vector2[_vertices.Count];

        float multiplier = 1 / (boardParameters.cellSize * boardParameters.gridSize);
        
        for (int i = 0; i < _uv.Length; i++)
        {
            _uv[i] = new Vector2(_vertices[i].x * multiplier, _vertices[i].z * multiplier);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
