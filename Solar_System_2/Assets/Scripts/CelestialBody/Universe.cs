using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NBodySimulation))]
public class Universe : MonoBehaviour
{
    
    //UniverseInfo
    public CelestialBody[] m_allCelestialBodies;

    public static Universe Instance{get; private set;}


    private void OnEnable()
    {
        
    }

    //Awake is called when scene is loaded
    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        Initialize();
    }

    // Start is called before the first frame update
    private void Start(){
        
    }

    //
    public void Initialize()
    {
        m_allCelestialBodies = FindObjectsOfType<CelestialBody>();
    }

    // Update is called once per frame
    private void Update()
    {
           
    }

    //FixedUpdate is called at fixed time period
    void FixedUpdate()
    {
        
    }

   

}
