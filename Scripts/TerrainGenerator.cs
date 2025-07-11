using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public BiomeGenerator biomeGenerator;
    public ChunkData GenerateChunkData(ChunkData chunkData, Vector3Int seedOffset)
    {
        for (int x = 0; x < chunkData.chunkSize; x++)
        {
            for (int z = 0; z < chunkData.chunkHeight; z++)
            {
                chunkData = biomeGenerator.ProcessData(chunkData, x, z, seedOffset);
            }
        }

        return chunkData;
    }
}