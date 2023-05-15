using UnityEngine;


[ExecuteInEditMode]
public class OrbitDebug : MonoBehaviour
{
    public int numSteps = 1000;
    public float timestep = 0.1f;
    public bool UsePhysicsTimeStep;
    private float G;

    public bool RelativeToBody;
    public CelestialBody CentralBody;

    public float lineThickness = 100f;
    public bool UseThickLines = true;

    private CelestialBody[] m_allCelestialBodies;


    private void Start()
    {
        if (Application.IsPlaying(this))
        {
           //HideOrbit();
        }
    }

    private void LateUpdate()
    {
        DrawOrbit();       
    }

    void DrawOrbit()
    {
        m_allCelestialBodies = FindObjectsOfType<CelestialBody>();
        var virtualBodies = new VirtualBody[m_allCelestialBodies.Length];
        var drawPoints = new Vector3[m_allCelestialBodies.Length][];

        int refferenceFrameIndex = 0;
        Vector3 refferenceBodyInitialPosition = Vector3.zero;

        for (int i = 0; i < m_allCelestialBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(m_allCelestialBodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (m_allCelestialBodies[i] == CentralBody && RelativeToBody)
            {
                refferenceFrameIndex = i;
                refferenceBodyInitialPosition = virtualBodies[i].position;
            }

        }
        
        for (int i = 0; i < virtualBodies.Length; i++)
            {
                drawPoints[i][0] = virtualBodies[i].position;
            }

        for (int step = 1; step < numSteps; step++)
        {
            Vector3 refferenceBodyPosition = (RelativeToBody) ? virtualBodies[refferenceFrameIndex].position : Vector3.zero;
         

            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timestep;
                virtualBodies[i].position = newPos;

                if (RelativeToBody)
                {
                    var refferenceOffset = refferenceBodyPosition - refferenceBodyInitialPosition;
                    newPos -= refferenceOffset;
                }
                if (RelativeToBody && i == refferenceFrameIndex)
                {
                    newPos = refferenceBodyInitialPosition;
                }
                drawPoints[i][step] = newPos;
            }

            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcc(i, virtualBodies) * timestep;
            }

        }

        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            var pathColor = m_allCelestialBodies[bodyIndex].BaseColour;
            
           
           
                var lineRenderer = m_allCelestialBodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColor;
                lineRenderer.endColor = Color.black * 0f;
                lineRenderer.widthMultiplier = lineThickness;
           
           
        
        }


    }
    
    void HideOrbits()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++)
        {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
    }

    public void SetLineThickness(float linethickness) => lineThickness = linethickness;

    public void SetNumStep(float numSteps){
        this.numSteps = (int)(numSteps*2990 + 10);
    }

    private void OnValidate()
    {
        if (UsePhysicsTimeStep)
        {
            timestep = NBodySimulation.Instance.Delta_time;
        }
    }

    Vector3 CalculateAcc(int i, VirtualBody[] virtualBodies)
    {
        Vector3 accerlation = Vector3.zero;

        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
                continue;

            Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position);
            float sqrDis = forceDir.sqrMagnitude;
            accerlation += forceDir.normalized * (NBodySimulation.Instance.G * virtualBodies[j].mass )/ sqrDis;
        }
        return accerlation;
    }

    class VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody(CelestialBody body)
        {
            position = body.transform.position;
            velocity = body.m_velocity;
            mass = body.m_mass;

        }

    }
}
