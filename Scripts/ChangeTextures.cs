using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextures : MonoBehaviour
{
    public GameObject world;
    public Material overworld;
    public Material nether;
    public Material lava;
    public Material water;
    public Material overworld_sky;
    public Material nether_sky;
    public Color overworld_fog;
    public Color nether_fog;

    public float overworldFogDensity;
    public float netherFogDensity;
    MeshRenderer[] children;
    bool ov;
    // Start is called before the first frame update

    void Start()
    {
        ov = true;

    }

    void LateStart()
    {
        ov = true;
        children = world.GetComponentsInChildren<MeshRenderer>();
    }

    

    private void changeDimensions()
    {
        foreach (var child in children)
        {
            Material[] materials = child.materials;
            if (ov)
            {
                materials[0] = overworld;
                materials[1] = water;

                RenderSettings.skybox = overworld_sky;
                RenderSettings.fogColor = overworld_fog;
                RenderSettings.fogDensity = overworldFogDensity;
                
            }
            else
            {
                materials[0] = nether;
                materials[1] = lava;
                RenderSettings.skybox = nether_sky;
                RenderSettings.fogColor = nether_fog;
                RenderSettings.fogDensity = netherFogDensity;
            }

            child.materials = materials;
        }
                
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            ov = !ov;
            children = world.GetComponentsInChildren<MeshRenderer>();
            changeDimensions();
        }

    }
}
