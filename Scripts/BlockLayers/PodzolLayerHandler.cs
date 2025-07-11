using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodzolLayerHandler : BlockLayerHandler
{
    [Range(0, 1)]
    public float threshold = 0.5f;

    public NoiseSettings noiseSettings;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight, int waterHeightThreshold, Vector3Int seedOffset)
    {
        if (chunkData.worldPosition.y > surfaceHeight) return false;

        noiseSettings.worldOffset = seedOffset;
        float noise = Noise.OctavePerlin(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, noiseSettings);

        int endPosition = surfaceHeight;

        if (chunkData.worldPosition.y < 0)
        {
            endPosition = chunkData.worldPosition.y + chunkData.chunkHeight;
        }
        if (noise > threshold)
        {
            if (endPosition >= waterHeightThreshold -1)
            {
                Vector3Int pos = new Vector3Int(x, endPosition, z);
                Chunk.SetBlock(chunkData, pos, BlockType.Podzol);
                return true;
            }
        }
        return false;
    }
}
