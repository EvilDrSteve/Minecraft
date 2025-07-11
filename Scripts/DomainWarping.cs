using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DomainWarping : MonoBehaviour
{
    public int numberOfWarps = 1;
    [Range(0f, 1f)]
    public float fallOff = 0.5f;
    public float amplitude = 1;
    public NoiseSettings noiseSettingsX;
    public NoiseSettings noiseSettingsZ;

    public float GetWarpedNoise(float x, float z, NoiseSettings biomeNoiseSettings)
    {
        float scaleX = noiseSettingsX.scale;
        float scaleZ = noiseSettingsZ.scale;
        for(int i = 0; i < numberOfWarps; i++)
        {
            float dx = Noise.RemapValue(Noise.OctavePerlin(x, z, noiseSettingsX), 0, 1, -1, 1) * amplitude;
            float dz = Noise.RemapValue(Noise.OctavePerlin(x, z, noiseSettingsZ), 0, 1, -1, 1) * amplitude;

            x += dx;
            z += dz;

            scaleX *= fallOff;
            scaleZ *= fallOff;
        }

        return Noise.OctavePerlin(x, z, biomeNoiseSettings);
    }
}
