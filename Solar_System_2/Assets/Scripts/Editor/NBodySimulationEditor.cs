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
            nBodySimulation.OnEnable();
        }
    }

    private void OnEnable(){
        nBodySimulation = (NBodySimulation)target;
    }
}
