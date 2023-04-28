using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    //Const
    public float G = 6;
    public int TimeWarp = 1;
    public float Delta_time = 0.01f;
    private float delta_time = 0.01f;

    //Stats
    public float Energy;
    public Vector3 Momentum;

    //UniverseInfo
    public CelestialBody[] m_allCelestialBodies;
    private float[] m_masses;
    private Vector3[] m_velocities;
    private Vector3[] m_positions;
    //private Vector3[] m_accerlations;
    private Vector3[] m_prevPositions;
    private float[,] m_massratios;
    private int numOfBodies;



    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize(){

        m_allCelestialBodies = FindObjectsOfType<CelestialBody>();
        numOfBodies = m_allCelestialBodies.Length;
        m_masses = new float[numOfBodies];
        m_positions = new Vector3[numOfBodies];
        m_prevPositions = new Vector3[numOfBodies];
        m_velocities = new Vector3[numOfBodies];
        //m_accerlations = new Vector3[numOfBodies];
        m_massratios = new float[numOfBodies,numOfBodies];

        for (int i = 0; i < numOfBodies; i++)
        {
            m_masses[i] = m_allCelestialBodies[i].m_mass;
            m_velocities[i] = m_allCelestialBodies[i].m_velocity;
            m_positions[i] = m_allCelestialBodies[i].transform.position;
            m_prevPositions[i] = m_allCelestialBodies[i].transform.position;

            
            //m_accerlations[i] = CalculateAcc(i,m_positions);
        }

        for (int i = 0; i < numOfBodies; i++)
        {
            for (int j = i+1; j < numOfBodies; j++)
            {
                m_massratios[i,j] = m_masses[i]/m_masses[j];
                m_massratios[j,i] = 1/ m_massratios[i,j];
            }
        }
        delta_time = Delta_time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Gravity Simulation 
        for (int n = 0; n < TimeWarp; n++)
        {       
            NBodySimulation();
        }        
    }

    private void Update()
    {
        if(Input.GetKeyDown("space")) {
            if(delta_time == 0) delta_time = Delta_time;
            else delta_time = 0;
        }

        

        for (int i = 0; i < numOfBodies; i++)
        {
            m_allCelestialBodies[i].UpdatePosition(m_positions[i]);
            m_allCelestialBodies[i].m_velocity = m_velocities[i];
        }

        CalculateTotalEnergy();
        CalculateMomentum();        
    }

    void NBodySimulation(){
        Vector3[] PrevAccs = CalculateAccelerations(m_prevPositions);
            Vector3[] CurrAccs = CalculateAccelerations(m_positions);

            for (int i = 0; i < numOfBodies; i++)
                {
                    Vector3 newPos = m_positions[i] + m_velocities[i] * delta_time + ((4*CurrAccs[i] - PrevAccs[i]) * delta_time * delta_time)/6;                    
                    //Debug.Log(CurrAccs[i] * 100);
                    m_prevPositions[i] = m_positions[i];
                    m_positions[i] = newPos;
                }
        
            Vector3[] NewAccs = CalculateAccelerations(m_positions);

            for (int i = 0; i < numOfBodies; i++){

                m_positions[i] = m_prevPositions[i] + m_velocities[i] * delta_time + ((2*CurrAccs[i] + NewAccs[i]) * delta_time * delta_time)/6;

                m_velocities[i] += (2*CurrAccs[i] + NewAccs[i]) * delta_time/3;

                //m_velocities[i] = (m_positions[i] - m_prevPositions[i])/delta_time ;


            }   
    }

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

    void CalculateTotalEnergy(){
        float newEnergy = 0;
        for (int i = 0; i < numOfBodies; i++)
        {
            for (int j = i+1; j < numOfBodies; j++)
            {
                float Dis = (m_positions[i] - m_positions[j]).magnitude;

                newEnergy += -G * m_masses[i]*m_masses[j]/Dis;
            }

            newEnergy += 0.5f * G * m_masses[i] * m_velocities[i].sqrMagnitude;
        }

        Energy = newEnergy;
    }

    void CalculateMomentum(){
        Vector3 newMomentum = Vector3.zero;
        for (int i = 0; i < numOfBodies; i++)
        {
            newMomentum +=m_masses[i] * m_velocities[i];
        }

        Momentum = newMomentum;
    }
}
