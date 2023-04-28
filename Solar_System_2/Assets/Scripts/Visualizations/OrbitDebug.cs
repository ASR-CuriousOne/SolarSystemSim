using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class OrbitDebug : MonoBehaviour
{
    public int numSteps = 1000;
    public float timestep = 0.1f;
    public bool UsePhysicsTimeStep;
    public bool call;

    public bool RelativeToBody;
    public CelestialBody CentralBody;

    public float lineThickness = 100f;
    public bool UseThickLines = true;

    public Universe universe = new Universe();


    private void Start()
    {
        if (Application.IsPlaying(this))
        {
           //HideOrbit();
        }
    }

    private void Update()
    {
        if(call)
            DrawOrbit();
        
    }

    void DrawOrbit()
    {
        CelestialBody[] allOtherBodies = FindObjectsOfType<CelestialBody>();
       
        var virtualBodies = new VirtualBody[allOtherBodies.Length];
        var drawPoints = new Vector3[allOtherBodies.Length][];

        int refferenceFrameIndex = 0;
        Vector3 refferenceBodyInitialPosition = Vector3.zero;

        for (int i = 0; i < allOtherBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(allOtherBodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (allOtherBodies[i] == CentralBody && RelativeToBody)
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
            var pathColor = allOtherBodies[bodyIndex].BaseColour;
            
           
           
                var lineRenderer = allOtherBodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
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



    private void OnValidate()
    {
        if (UsePhysicsTimeStep)
        {
            timestep = universe.Delta_time;
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
            accerlation += forceDir.normalized * (universe.G * virtualBodies[j].mass )/ sqrDis;
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
