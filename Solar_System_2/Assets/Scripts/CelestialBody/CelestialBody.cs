using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphericalMesh))]
public class CelestialBody : MonoBehaviour
{
    SphericalMesh m_sphereMesh;

    public float m_mass  = 1;

    public Vector3 m_velocity = Vector3.zero;    

    public Color BaseColour = new Color(0,0,0,1);

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_sphereMesh = gameObject.GetComponent<SphericalMesh>();
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }

    public void UpdatePosition(Vector3 new_velocity,float delta_time){

        transform.position += m_velocity * delta_time;

        m_velocity = new_velocity;
    }

    public void UpdatePosition(Vector3 position){
        transform.position = position;
    }

    public class CelestialBodyData{
        
    }



}
