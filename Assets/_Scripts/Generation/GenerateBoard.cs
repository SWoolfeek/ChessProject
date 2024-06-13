using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour
{
    [SerializeField] private float cellSize;
    [SerializeField] private int _gridSize = 8;

    [SerializeField] private Material material;
    
    readonly Vector3 _startingPoint = Vector3.zero;
    
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _tris = new List<int>();
    private List<Vector3> _normals = new List<Vector3>();
    private Vector2[] _uv;
    
    // Start is called before the first frame update
    void Start()
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
        int verticesAmount = (_gridSize + 1)*(_gridSize + 1);
        
        for (int i = 0; i < verticesAmount; i++)
        {
            int row = i / (_gridSize + 1);
            int column = i % (_gridSize + 1);
            _vertices.Add(_startingPoint + Vector3.forward * row * cellSize + Vector3.right * column * cellSize);
        }
        
        Debug.Log("Vertices - " + _vertices);
    }

    private void GenerateTris()
    {
        for (int i = 0; i < (_gridSize * _gridSize); i++)
        {
            int row = i / (_gridSize);
            int column = i % (_gridSize);
            
            //First tris
            _tris.Add(0 + column + row * (_gridSize + 1));
            _tris.Add(_gridSize + column + 1 + row * (_gridSize + 1));
            _tris.Add(1 + column + row * (_gridSize + 1));
            
            
            //Second tris
            _tris.Add(_gridSize + column + 1 + row * (_gridSize + 1));
            _tris.Add(_gridSize + column + 2 + row * (_gridSize + 1));
            _tris.Add(1 + column + row * (_gridSize + 1));
            
            print("Row - " + row + " Column - " + column);
        }
    }

    private void GenerateNormals()
    {
        for (int i = 0; i < (_gridSize + 1)*(_gridSize + 1); i++)
        {
            _normals.Add(-Vector3.up);
        }
    }

    private void GenerateUv()
    {
        _uv = new Vector2[_vertices.Count];

        float multiplier = 1 / (cellSize * _gridSize);
        
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
