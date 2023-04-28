using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SphericalMesh : MonoBehaviour
{
    [Range(0,100)]
    public int sub_divisions;

    [SerializeField,HideInInspector]
    MeshFilter[] meshFilters;

    [SerializeField,HideInInspector]
    GameObject[] m_Quads;

    [SerializeField,HideInInspector]
    GameObject m_Quads_Parent;

    public Material mat;

    private void OnEnable()
    {
        Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (m_Quads_Parent == null) 
        { 
            m_Quads_Parent = new GameObject("Quads'_Parent");
            m_Quads_Parent.transform.parent = gameObject.transform;
            m_Quads_Parent.transform.localPosition = Vector3.zero ;
            m_Quads_Parent.transform.localRotation = Quaternion.Euler(0, 0, 0);
            m_Quads_Parent.transform.localScale = Vector3.one;
        }
        if (m_Quads == null || m_Quads.Length != 8)
        {
            m_Quads = new GameObject[8];
            meshFilters = new MeshFilter[8];
            for (int i = 0; i < 8; i++)
            {
                m_Quads[i] = new GameObject("Quad - " + i.ToString());
                m_Quads[i].transform.parent = m_Quads_Parent.transform;
                m_Quads[i].transform.localPosition = Vector3.zero;
                m_Quads[i].transform.localRotation = Quaternion.Euler(0, 0, 0);
                m_Quads[i].transform.localScale = Vector3.one;
                m_Quads[i].AddComponent<MeshFilter>();
                m_Quads[i].AddComponent<MeshRenderer>().sharedMaterial = mat;
                meshFilters[i] = m_Quads[i].GetComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
        }

        CreateNewMesh();
    }

    public void DestroyAllQuads()
    {
        if (m_Quads != null )
        {
            if(m_Quads.Length != 0){
                foreach (GameObject Quad in m_Quads)
                {
                    if (Application.isPlaying) Destroy(Quad);
                    else DestroyImmediate(Quad);
                }
            }
        }
        m_Quads = null;
    }

    public void CreateNewMesh(){

        Vector3[] triangleFace = new Vector3[3] { new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1) };
        Vector3Int[] Transformations = new Vector3Int[7] { new Vector3Int(-1, -1, 1), new Vector3Int(1, -1, -1), new Vector3Int(-1, 1, -1), new Vector3Int(-1, 1, 1), new Vector3Int(1, -1, 1), new Vector3Int(1, 1, -1), new Vector3Int(-1, -1, -1) };

        sub_divisions = sub_divisions < 1 ? 0 : sub_divisions;
        

        MeshData meshData = SubdivideTriangle(sub_divisions, triangleFace);

        Vector3[] verticesSingleFace = meshData.verts;
        int[] trianglesSingleFace = meshData.trigs;
       
        meshFilters[0].sharedMesh.Clear();
        meshFilters[0].sharedMesh.vertices = verticesSingleFace;
        meshFilters[0].sharedMesh.triangles = trianglesSingleFace;
        meshFilters[0].sharedMesh.normals = verticesSingleFace;
        meshFilters[0].sharedMesh.uv = CalculateUVs(verticesSingleFace);
       

        for (int i = 1; i < 4; i++)
        {
            Vector3[] transformed = Transformation(verticesSingleFace, Transformations[i - 1]);
            meshFilters[i].sharedMesh.Clear();
            meshFilters[i].sharedMesh.vertices = transformed;
            meshFilters[i].sharedMesh.triangles = trianglesSingleFace;
            meshFilters[i].sharedMesh.normals = transformed;
            meshFilters[i].sharedMesh.uv = CalculateUVs(transformed);
        }

        ReverseTrigs(ref trianglesSingleFace);

        for (int i = 4; i < 8; i++)
        {
            Vector3[] transformed = Transformation(verticesSingleFace, Transformations[i - 1]);
            meshFilters[i].sharedMesh.Clear();
            meshFilters[i].sharedMesh.vertices = transformed;
            meshFilters[i].sharedMesh.triangles = trianglesSingleFace;
            meshFilters[i].sharedMesh.normals = transformed;
            meshFilters[i].sharedMesh.uv = CalculateUVs(transformed);

        }
        
    }

    public void ChangeTexture(string TextureName,Texture2D texture)
    {
        mat.SetTexture(TextureName, texture);
    }

    Vector3 interpolate(Vector3 vec_1, Vector3 vec_2, Vector3 vec_3, float w_1, float w_2, float w_3)
    {
        return new Vector3((vec_1.x * w_1 + vec_2.x * w_2 + vec_3.x * w_3), (vec_1.y * w_1 + vec_2.y * w_2 + vec_3.y * w_3), (vec_1.z * w_1 + vec_2.z * w_2 + vec_3.z * w_3));
    }

    Vector3[] Transformation(Vector3[] verts,Vector3Int tranformations)
    {
        Vector3[] verts2 = new Vector3[verts.Length];

        for (int i = 0; i < verts2.Length; i++)
        {
            verts2[i] = new Vector3(verts[i].x * tranformations.x, verts[i].y * tranformations.y, verts[i].z * tranformations.z);
        }

        return verts2;
    }

    void ReverseTrigs(ref int[] trigs)
    {
        
        for (int i = 0; i < trigs.Length; i += 3)
        {
            int temp = trigs[i];

            trigs[i] = trigs[i + 2];
            trigs[i + 1] = trigs[i + 1];
            trigs[i + 2] = temp;
        }
            
        
    }

    MeshData SubdivideTriangle(int sub_divisions,Vector3[] TrianglePoints) 
    {
        Vector3[] vertices = new Vector3[((sub_divisions + 2) * (sub_divisions + 2 + 1) / 2)];
        int[] triangles = new int[((sub_divisions + 1) * (sub_divisions + 1) * 3)];

        vertices[0] = TrianglePoints[0];

        for (int row = 1; row < sub_divisions + 2; row++)
        {
            float w_1 = 1.0f - ((float)row / (float)(sub_divisions + 1));

            for (int point_on_row = 0; point_on_row < (row + 1); point_on_row++)
            {
                float w_2 = (1.0f - w_1) * (1.0f - ((float)point_on_row / (float)(row)));
                float w_3 = (1.0f - w_1 - w_2);

                vertices[(row) * (row + 1) / 2 + point_on_row] = interpolate(TrianglePoints[0], TrianglePoints[1], TrianglePoints[2], w_1, w_2, w_3).normalized;
            }
        }

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        int TriangleIndex = 2;
        int current_point_index = 0;

        for (int row = 1; row < sub_divisions + 1; row++)
        {

            for (int point_on_row = 0; point_on_row < (row); point_on_row++)
            {
                current_point_index = row * (row + 1) / 2 + point_on_row;

                triangles[TriangleIndex + 1] = current_point_index;
                triangles[TriangleIndex + 2] = current_point_index + row + 2;
                triangles[TriangleIndex + 3] = current_point_index + row + 1;

                triangles[TriangleIndex + 4] = current_point_index;
                triangles[TriangleIndex + 5] = current_point_index + 1;
                triangles[TriangleIndex + 6] = current_point_index + row + 2;

                TriangleIndex += 6;
            }

            current_point_index = row * (row + 3) / 2;

            triangles[TriangleIndex + 1] = current_point_index;
            triangles[TriangleIndex + 2] = current_point_index + row + 2;
            triangles[TriangleIndex + 3] = current_point_index + row + 1;

            TriangleIndex += 3;
        }
        return new MeshData(vertices,triangles);
        
    }

    Vector2[] CalculateUVs(Vector3[] verts)
    {
        Vector2[] uvs = new Vector2[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            uvs[i] = new Vector2((.5f + (Mathf.Atan2(verts[i].x, -verts[i].z) / (2 * Mathf.PI))) ,(.5f + Mathf.Asin(verts[i].y)/ (Mathf.PI)));
        }
        return uvs;
    }

    struct MeshData
    {
        public Vector3[] verts;
        public int[] trigs;

        public MeshData(Vector3[] _verts, int[] _trigs)
        {
            verts = _verts;
            trigs = _trigs;
        }
    }

   
}
