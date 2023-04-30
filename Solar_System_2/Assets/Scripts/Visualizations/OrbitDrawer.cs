using UnityEngine;

public class OrbitDrawer : MonoBehaviour
{
    Vector3[][] drawPoints;
    
    private CelestialBody[] m_allCelestialBodies;

    //SimulationVariables
    private float[] m_masses;
    private Vector3[] m_velocities;
    private Vector3[] m_positions;
    private Vector3[] m_prevPositions;
    private float[,] m_massratios;
    private int numOfBodies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Initialize(){
        m_allCelestialBodies = Universe.Instance.m_allCelestialBodies;
    }

    
}
