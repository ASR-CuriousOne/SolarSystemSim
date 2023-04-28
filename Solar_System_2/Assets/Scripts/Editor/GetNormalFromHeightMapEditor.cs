using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GetNormalMapFromHeightMap))]
public class GetNormalFromHeightMapEditor : Editor
{
    GetNormalMapFromHeightMap getNormalMapFromHeightMap;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Make Normal Map")) getNormalMapFromHeightMap.ConverterFromHeightToNormalsFunc(false);

        if (GUILayout.Button("Make Normal Map and Save"))
        {
            getNormalMapFromHeightMap.ConverterFromHeightToNormalsFunc(true);
            
        }

    }

    private void OnEnable()
    {
        getNormalMapFromHeightMap = (GetNormalMapFromHeightMap)target;   
    }
}
