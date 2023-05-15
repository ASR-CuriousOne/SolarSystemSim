using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Universe))]
public class UniverseEditor : Editor
{
    Universe universe;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Make Celestial Body")){
            universe.CreateCelestialBody(Vector3.forward * 69f,1f,Random.ColorHSV(),"Default");
        }
    }

    private void OnEnable(){
        universe = (Universe)target;
    }
}
