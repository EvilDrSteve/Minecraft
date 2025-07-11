using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneLayerHandler : BlockLayerHandler
{
    [Range(0, 1)]
    public float stoneThreshold = 0.5f;

    public NoiseSettings stoneNoiseSettings;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight, int waterHeightThreshold, Vector3Int seedOffset)
    {
        if (chunkData.worldPosition.y > surfaceHeight) return false;

        stoneNoiseSettings.worldOffset = seedOffset;
        float stoneNoise = Noise.OctavePerlin(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, stoneNoiseSettings);

        int endPosition = surfaceHeight;

        if(chunkData.worldPosition.y < 0)
        {
            endPosition = chunkData.worldPosition.y + chunkData.chunkHeight;
        }
        if(stoneNoise > stoneThreshold)
        {
            for(int i = 0; i <= endPosition; i++)
            {
                Vector3Int pos = new Vector3Int(x, i, z);
                Chunk.SetBlock(chunkData, pos, BlockType.Stone);
            }

            return true;
        }
        return false;
    }
}
