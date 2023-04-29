using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SphericalMesh))]
public class SphericalMeshEditor : Editor
{
    SphericalMesh m_sphericalMesh;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reinitialize"))
        {
            m_sphericalMesh.DestroyAllQuads();
            m_sphericalMesh.Initialize();
        }

        if (GUILayout.Button("Generate Mesh"))
        {
            m_sphericalMesh.CreateNewMesh();
        }

        if (GUILayout.Button("Destroy All Quads"))
        {
            m_sphericalMesh.DestroyAllQuads();
        }

        
    }

    private void OnEnable()
    {
        m_sphericalMesh = (SphericalMesh)target;
    }
}
