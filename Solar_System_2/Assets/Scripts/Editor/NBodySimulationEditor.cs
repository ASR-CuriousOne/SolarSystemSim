using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NBodySimulation))]
public class NBodySimulationEditor : Editor
{
    NBodySimulation nBodySimulation;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Make Instance")){
            nBodySimulation.Awake();
        }
    }

    private void OnEnable(){
        nBodySimulation = (NBodySimulation)target;
    }
}
