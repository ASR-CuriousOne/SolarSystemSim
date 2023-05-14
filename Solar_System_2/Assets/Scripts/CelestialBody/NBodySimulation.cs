using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    private CelestialBody[] m_allCelestialBodies;

    public static NBodySimulation Instance{get; private set;} 

    //SimulationConstants
    public float G = 6;
    public int TimeWarp = 1;
    public float Delta_time = 0.02f;

    //Stats
    public float Energy;
    public Vector3 Momentum;

    //UniverseInfo
    private float[] m_masses;
    private Vector3[] m_velocities;
    private Vector3[] m_positions;
    private Vector3[] m_prevPositions;
    private float[,] m_massratios;
    private int numOfBodies;

    private bool IsPaused = true;

    
    //Enable is called
    public void OnEnable()
    {
    }

    //Awake is called when scene is loaded
    public void Awake()
    {
        if(Instance != null && (Instance != this)) Destroy(this);
        else Instance = this;      

    }

    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown("space")) 
        {
            if(IsPaused) PlaySimulation();
            else PauseSimuation();
        }  

        CalculateTotalEnergy();
        CalculateTotalMomentum();            

        if(IsPaused) return;
        for (int i = 0; i < numOfBodies; i++)
        {
            m_allCelestialBodies[i].UpdatePosition(m_positions[i]);
            m_allCelestialBodies[i].m_velocity = m_velocities[i];
        }  
    }

    //FixedUpdate is called at fixed time period
    private void FixedUpdate()
    {
        if(IsPaused) return;
        // Gravity Simulation 
        for (int n = 0; n < TimeWarp; n++)
        {       
            GravSimulation();
        }        
    }

    //Initialize simuation variables
    public void Initialize()
    {
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

    //Simulate celestial bodies 
    void GravSimulation()
    {
            Vector3[] PrevAccs = CalculateAccelerations(m_prevPositions);
            Vector3[] CurrAccs = CalculateAccelerations(m_positions);

            for (int i = 0; i < numOfBodies; i++)
                {
                    Vector3 newPos = m_positions[i] + m_velocities[i] * Delta_time + ((4*CurrAccs[i] - PrevAccs[i]) * Delta_time * Delta_time)/6.0f;
                    m_prevPositions[i] = m_positions[i];
                    m_positions[i] = newPos;
                }
        
            Vector3[] NewAccs = CalculateAccelerations(m_positions);

            for (int i = 0; i < numOfBodies; i++){

                m_positions[i] = m_prevPositions[i] + m_velocities[i] * Delta_time + ((2.0f*CurrAccs[i] + NewAccs[i]) * (Delta_time * Delta_time))/6.0f;

                m_velocities[i] += (2.0f*CurrAccs[i] + NewAccs[i]) * Delta_time/3.0f;
            }   
    }

    //Calculate accerlation for a single body
    Vector3 CalculateAcc(int i, Vector3[] positions)
    {
        Vector3 accerlation = Vector3.zero;
        for (int j = 0; j < positions.Length; j++)
        {
            if (i == j)
                continue;
            Vector3 forceDir = (positions[j] - positions[i]);
            float Dis = forceDir.magnitude;
            accerlation += forceDir * ((G * m_masses[j] )/ (Dis*Dis*Dis)); 
        }
        return accerlation;
    }

    //Calculate accerlations for all bodies
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

    //Calculate total energy of the system
    void CalculateTotalEnergy(){
        float newEnergy = 0;
        for (int i = 0; i < numOfBodies; i++)
        {
            for (int j = i+1; j < numOfBodies; j++)
            {
                float Dis = (m_positions[i] - m_positions[j]).magnitude;

                newEnergy += -G * m_masses[i]*m_masses[j]/Dis;
            }

            newEnergy += 0.5f * m_masses[i] * m_velocities[i].sqrMagnitude;
        }

        Energy = newEnergy;
    }

    //Calculate total momentum of the system
    void CalculateTotalMomentum(){
        Vector3 newMomentum = Vector3.zero;
        for (int i = 0; i < numOfBodies; i++)
        {
            newMomentum +=m_masses[i] * m_velocities[i];
        }

        Momentum = newMomentum;
    }

    public void PauseSimuation(){
        IsPaused = true;
    }

    public void PlaySimulation(){

        for (int i = 0; i < numOfBodies; i++)
        {
            m_masses[i] = m_allCelestialBodies[i].m_mass;
            m_velocities[i] = m_allCelestialBodies[i].m_velocity;
            m_positions[i] = m_allCelestialBodies[i].transform.position;
            m_prevPositions[i] = m_allCelestialBodies[i].transform.position;
        }

        IsPaused = false;
    }

    public void SetWarpSpeed(float warpSpeed){
        TimeWarp = (int)(warpSpeed * 1680 + 10);
    }

}
