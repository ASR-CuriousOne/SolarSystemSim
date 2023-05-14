using UnityEngine;

[RequireComponent(typeof(SphericalMesh),typeof(LineRenderer))]
public class CelestialBody : MonoBehaviour
{
    SphericalMesh m_sphereMesh;
    LineRenderer m_orbitLine;

    public float m_mass  = 1;

    public Vector3 m_velocity = Vector3.zero;    

    public Color BaseColour = new Color(0,0,0,1);

    public float orbitThickness = .3f;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_orbitLine = gameObject.GetComponent<LineRenderer>();
        m_sphereMesh = gameObject.GetComponent<SphericalMesh>();

        m_orbitLine.startColor = BaseColour;
        m_orbitLine.endColor = Color.black * 0f;
        m_orbitLine.widthMultiplier = orbitThickness;
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

    public void UpdateOrbit(Vector3[] orbitPoints){
        m_orbitLine.positionCount = orbitPoints.Length;
        m_orbitLine.widthMultiplier = orbitThickness;
        m_orbitLine.SetPositions(orbitPoints);
        
    }



}
