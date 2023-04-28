using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNormalMapFromHeightMap : MonoBehaviour
{
    public SphericalMesh moon;

    public Texture2D heightMap;

    public RenderTexture normalMap;

    const string LocationToSave = "C:/Users/BSNL/Desktop/";

    public ComputeShader ConverterFromHeightToNormals;

    public float heightMultiplier;

    public void ConverterFromHeightToNormalsFunc(bool save) 
    {        
        normalMap = new RenderTexture(heightMap.width, heightMap.height,1);

        normalMap.enableRandomWrite = true;

        normalMap.Create();
        
        ConverterFromHeightToNormals.SetTexture(0, "HeightMap", heightMap);
        ConverterFromHeightToNormals.SetTexture(0, "NormalMap", normalMap);
        ConverterFromHeightToNormals.SetFloat("MapWidth", heightMap.width);
        ConverterFromHeightToNormals.SetFloat("MapHeight", heightMap.height);
        ConverterFromHeightToNormals.SetFloat("HeightMultiplier", heightMultiplier);

        ConverterFromHeightToNormals.Dispatch(0, heightMap.width / 32, heightMap.height / 32, 1);



        if (save)
        {
            byte[] bytes = toTexture2D(normalMap).EncodeToPNG();
            System.IO.File.WriteAllBytes(LocationToSave + "NormalMap.png", bytes);
        }
        Debug.Log("Done");
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height ,TextureFormat.RGB24 , false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public void ChangeNormalMap()
    {
        Texture2D TextureToChange = toTexture2D(normalMap);
        

        moon.ChangeTexture("_BumpMap", TextureToChange);
    }

}
