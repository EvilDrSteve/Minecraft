using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public int waterHeightThreshold = 50;
    public bool useDomainWarp;
    public bool useTerracedDistribution;
    public NoiseSettings noiseSettings;
    public NoiseSettings terraceNoiseSettings;
    public DomainWarping domainWarping;
    public BlockLayerHandler startLayerHandler;

    public List<BlockLayerHandler> additionalLayerHandlers;
    public ChunkData ProcessData(ChunkData chunkData, int x, int z, Vector3Int seedOffset)
    {
        noiseSettings.worldOffset = seedOffset;
        int groundPosition = GetSurfaceHeightNoise(chunkData.worldPosition.x + x,chunkData.worldPosition.z + z, chunkData.chunkHeight);

        for (int y = 0; y < chunkData.chunkHeight; y++)
        {
            startLayerHandler.Handle(chunkData, x, y, z, groundPosition, waterHeightThreshold, seedOffset);
            foreach(BlockLayerHandler handler in additionalLayerHandlers)
            {
                handler.Handle(chunkData, x,  chunkData.worldPosition.y, z, groundPosition, waterHeightThreshold, seedOffset);
            }
        }
        return chunkData;
    }

    private int GetSurfaceHeightNoise(int x, int z, int chunkHeight)
    {
        float terrainHeight = Noise.OctavePerlin(x, z, noiseSettings);
        if(useDomainWarp)
        terrainHeight = (domainWarping.GetWarpedNoise(x, z, noiseSettings));
        terrainHeight = Noise.Redistribute(terrainHeight, noiseSettings);
        if (useTerracedDistribution)
        {
            terrainHeight = Noise.StepDistribute(terrainHeight, noiseSettings, terraceNoiseSettings);
            terrainHeight = Noise.Redistribute(terrainHeight, terraceNoiseSettings);
        }
        int surfaceHeight = Noise.RemapValue01ToInt(terrainHeight, 0, chunkHeight);
        return surfaceHeight;
    }
}