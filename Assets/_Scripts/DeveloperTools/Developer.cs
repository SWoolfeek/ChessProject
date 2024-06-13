using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if  UNITY_EDITOR
using UnityEditor;
#endif


public class Developer
{
    
#if  UNITY_EDITOR
    [MenuItem("Developer/Clear Saves")]
    public static void ClearSaves()
    {
        Debug.Log("All save have been cleared");
    }
#endif
}
