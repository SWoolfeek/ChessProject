using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Generation/BoardParameters")]
public class BoardParameters : ScriptableObject
{
    [Min(1)] public int gridSize = 8;
    [Min(1)] public float cellSize = 2;
}
