using UnityEngine;

public class OrbitDrawer : MonoBehaviour
{
    public float delta_time;
    [Range(1,3000)]
    public int numOfSteps;
    public CelestialBody refrenceBody;

    Vector3[][] drawPoints;
    
    private CelestialBody[] m_allCelestialBodies;

    public float thicknessMultiplier = 0.3f;

    //SimulationVariables
    private float G;
    private float[] m_masses;
    private Vector3[] m_velocities;
    private Vector3[] m_positions;
    private Vector3[] m_prevPositions;
    private float[,] m_massratios;
    private int numOfBodies;

    // Start is called before the first frame update
    void Start(){
        Initialize();
    }

    void LateUpdate(){
        DrawOrbits();        
    }

    void Initialize(){
        G = NBodySimulation.Instance.G;

        m_allCelestialBodies = Universe.Instance.m_allCelestialBodies;
        numOfBodies = m_allCelestialBodies.Length;
        m_masses = new float[numOfBodies];
        m_positions = new Vector3[numOfBodies];
        m_prevPositions = new Vector3[numOfBodies];
        m_velocities = new Vector3[numOfBodies];
        m_massratios = new float[numOfBodies,numOfBodies];

        for (int i = 0; i < numOfBodies; i++)
        {
            m_masses[i] = m_allCelestialBodies[i].m_mass;
            m_velocities[i] = m_allCelestialBodies[i].m_velocity;
            m_positions[i] = m_allCelestialBodies[i].transform.position;
            m_prevPositions[i] = m_allCelestialBodies[i].transform.position;
        }

        for (int i = 0; i < numOfBodies; i++)
        {
            for (int j = i+1; j < numOfBodies; j++)
            {
                m_massratios[i,j] = m_masses[i]/m_masses[j];
                m_massratios[j,i] = 1/ m_massratios[i,j];
            }
        }
    }

    void DrawOrbits(){

        drawPoints = new Vector3[numOfBodies][];

        for (int i = 0; i < numOfBodies; i++)
        {
            drawPoints[i] = new Vector3[numOfSteps];            

            m_velocities[i] = m_allCelestialBodies[i].m_velocity;
            m_positions[i] = m_allCelestialBodies[i].transform.position;
            m_prevPositions[i] = m_allCelestialBodies[i].transform.position;
        }

        for (int step = 0; step < numOfSteps; step++)
        {
            GravitySimulation(step);
        }

        for (int i = 0; i < numOfBodies; i++)
        {
            m_allCelestialBodies[i].UpdateOrbit(drawPoints[i],thicknessMultiplier);
        }
    }

    void GravitySimulation(int step){
        Vector3[] PrevAccs = CalculateAccelerations(m_prevPositions);
        Vector3[] CurrAccs = CalculateAccelerations(m_positions);
        for (int i = 0; i < numOfBodies; i++)
            {
                drawPoints[i][step] = m_positions[i];

                if(m_allCelestialBodies[i].IsAnchored) continue;

                Vector3 newPos = m_positions[i] + m_velocities[i] * delta_time + ((4*CurrAccs[i] - PrevAccs[i]) * delta_time * delta_time)/6.0f;
                m_prevPositions[i] = m_positions[i];
                m_positions[i] = newPos;
            }
    
        Vector3[] NewAccs = CalculateAccelerations(m_positions);

        for (int i = 0; i < numOfBodies; i++)
        {
            if(m_allCelestialBodies[i].IsAnchored) continue;

            m_positions[i] = m_prevPositions[i] + m_velocities[i] * delta_time + ((2.0f*CurrAccs[i] + NewAccs[i]) * (delta_time * delta_time))/6.0f;

            m_velocities[i] += (2.0f*CurrAccs[i] + NewAccs[i]) * delta_time/3.0f;
        }
    }

    Vector3[] CalculateAccelerations(Vector3[] positions){
    
        Vector3[] Accelerations = new Vector3[positions.Length];

        for (int i = 0; i < numOfBodies - 1; i++)
        {
            for (int j = i+1; j < numOfBodies; j++)
            {
                Vector3 R = positions[i] - positions[j];
                float Dis = R.magnitude;
                Vector3 Aij = R * ((G * m_masses[j])/( Dis * Dis * Dis ));
                Accelerations[i] -= Aij;
                Accelerations[j] += Aij * m_massratios[i,j];
            }
        }        

        return Accelerations;
    }

    public void SetLineThickness(float thickness)
    {
        thicknessMultiplier = thickness;
    }

    public void SetNumberOfSteps(float numberOfSteps) {
        numOfSteps = (int)(3000 * numberOfSteps);    
    }

}
