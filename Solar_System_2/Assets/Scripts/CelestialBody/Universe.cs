using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NBodySimulation))]
public class Universe : MonoBehaviour
{
    
    //UniverseInfo
    public CelestialBody[] m_allCelestialBodies;

    public static Universe Instance{get; private set;}


    void OnEnable()
    {
        if(Instance != null && !(Instance != this)) Destroy(this);
        else Instance = this;
    }
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
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
