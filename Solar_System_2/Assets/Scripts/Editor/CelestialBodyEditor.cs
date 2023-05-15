using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CelestialBody))]
public class CelestialBodyEditor : Editor
{
    CelestialBody celestialBody;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Initialize")) celestialBody.Initialize();
    }

    private void OnEnable(){
        celestialBody = (CelestialBody)target;
    }
}
