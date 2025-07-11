using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/NoiseSettings")]
public class NoiseSettings : ScriptableObject
{
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public float redistributionModifier;
    public float exponent;
    public Vector3 offset;
    public Vector3 worldOffset;
}