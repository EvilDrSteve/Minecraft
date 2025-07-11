using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class Noise
{ 
    private static Simplex.Noise simplex = new Simplex.Noise();
    public static float OctavePerlin(float x, float y, NoiseSettings noiseSettings)
    {
        x *= noiseSettings.scale;
        y *= noiseSettings.scale;
        x += noiseSettings.scale;
        y += noiseSettings.scale;

        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float amplitudeSum = 0;

        for(int i = 0; i < noiseSettings.octaves; i++)
        {
            total += Mathf.PerlinNoise((noiseSettings.offset.x + noiseSettings.worldOffset.x + x) * frequency, (noiseSettings.offset.y + noiseSettings.worldOffset.y + y) * frequency) * amplitude;

            amplitudeSum += amplitude;

            amplitude *= noiseSettings.persistence;
            frequency *= noiseSettings.lacunarity;
        }

        return total / amplitudeSum;
    }

    public static float OctaveSimplex3D(float x, float y, float z, NoiseSettings noiseSettings)
    {
        

        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float amplitudeSum = 0;

        for (int i = 0; i < noiseSettings.octaves; i++)
        {
            total += simplex.CalcPixel3D((int)((noiseSettings.offset.x + noiseSettings.worldOffset.x + x) * frequency), (int)((noiseSettings.offset.y + noiseSettings.worldOffset.y + y) * frequency), (int)((noiseSettings.offset.z + noiseSettings.worldOffset.z + z) * frequency), noiseSettings.scale) * amplitude;

            amplitudeSum += amplitude;

            amplitude *= noiseSettings.persistence;
            frequency *= noiseSettings.lacunarity;
        }

        return total / amplitudeSum;
    }


    public static float RemapValue(float value, float min, float max, float outMin, float outMax)
    {
        return outMin + (value - min) * (outMax - outMin) / (max - min);
    }

    public static float RemapValue01(float value, float outMin, float outMax)
    {
        return outMin + (value - 0) * (outMax - outMin) / (1 - 0);
    }

    public static int RemapValue01ToInt(float value, int outMin, int outMax)
    {
        return (int)RemapValue01(value, outMin, outMax);
    }

    public static float Redistribute(float noise, NoiseSettings noiseSettings)
    {
        return Mathf.Pow(noise * noiseSettings.redistributionModifier, noiseSettings.exponent);
    }
    public static float StepDistribute(float noise, NoiseSettings defaultNoiseSettings, NoiseSettings noiseSettings)
    {
        int steps = (int)Noise.RemapValue01((Noise.OctavePerlin(noise, noise, noiseSettings)), 12, 60);
        return Mathf.Round(noise * steps) / steps;
    }

}
