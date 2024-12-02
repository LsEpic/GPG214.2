using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinNoise))]
public class PerlinNoiseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerlinNoise perlinNoiseTarget = (PerlinNoise)target;

        if(GUILayout.Button("Generate Perlin Noise"))
        {
            perlinNoiseTarget.ApplyPerlinNoiseToTerrain();
        }
    }
}
    
