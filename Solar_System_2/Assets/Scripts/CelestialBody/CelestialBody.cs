using UnityEngine;

[RequireComponent(typeof(SphericalMesh),typeof(LineRenderer))]
public class CelestialBody : MonoBehaviour
{
    SphericalMesh m_sphereMesh;
    LineRenderer m_orbitLine;

    [Header("Body Properties")]
    public float m_mass  = 1;
    public Color BaseColour = new Color(0,0,0,1);
    public float orbitThickness = .9f;

    public Vector3 m_velocity = Vector3.zero;    

    [Header("Special Attributes")]
    public bool IsAnchored;

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

    public void UpdateOrbit(Vector3[] orbitPoints,float thicknessMultiplier){
        m_orbitLine.positionCount = orbitPoints.Length;
        m_orbitLine.widthMultiplier = orbitThickness * thicknessMultiplier;
        m_orbitLine.SetPositions(orbitPoints);
        
    }



}
