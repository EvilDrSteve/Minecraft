
using Simplex;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    public int mapSizeInChunks = 6;
    public int chunkSize = 16, chunkHeight = 100;

    public GameObject chunkPrefab;

    Dictionary<Vector3Int, ChunkData> chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>();
    Dictionary<Vector3Int, ChunkRenderer> chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>();

    public TerrainGenerator terrainGenerator;
    public Vector3Int seedOffset;

    public float baseLevel;
    public float seaLevel;
    public AnimationCurve ContinentSpline;
    public AnimationCurve ErosionSpline;
    public AnimationCurve PeaksAndValliesSpline;

    public NoiseSettings ContinentNoise;
    public NoiseSettings ErosionNoise;
    public NoiseSettings PVNoise;
    public NoiseSettings squashNoise;
    public NoiseSettings simplexNoise;

    public float simplexScale;
    public float squashingFactor;

    Simplex.Noise simplex = new Simplex.Noise();

    public void GenerateWorld()
    {
        chunkDataDictionary.Clear();
        foreach(ChunkRenderer chunk in chunkDictionary.Values)
        {
            Destroy(chunk.gameObject);
        }
        chunkDictionary.Clear();

        for(int x = 0; x < mapSizeInChunks; x++)
        {
            for(int z = 0; z < mapSizeInChunks; z++)
            {
                ChunkData chunkData = new ChunkData(chunkSize, chunkHeight, new Vector3Int(x * chunkSize, 0, z* chunkSize), this);
                // GenerateVoxels(chunkData);
                ChunkData newChunk = terrainGenerator.GenerateChunkData(chunkData, seedOffset);

                chunkDataDictionary.Add(newChunk.worldPosition, newChunk);
            }
        }

        foreach(ChunkData chunkData in chunkDataDictionary.Values)
        {
            MeshData meshData = Chunk.GetChunkMeshData(chunkData);
            GameObject chunkObject = Instantiate(chunkPrefab, chunkData.worldPosition, Quaternion.identity);
            chunkObject.transform.parent = GameObject.Find("World").transform;
            ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
            chunkDictionary.Add(chunkData.worldPosition, chunkRenderer);
            chunkRenderer.InitializeChunk(chunkData);
            chunkRenderer.UpdateChunk(meshData);
        }

    }

    public BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.chunkPositionFromBlockCoords(this, x, y, z);
        ChunkData containerChunk = null;

        chunkDataDictionary.TryGetValue(pos, out containerChunk);

        if(containerChunk == null) 
            return BlockType.None;

            Vector3Int blockInChunkCoordinates = Chunk.GetBlockChunkCoordinates(containerChunk, new Vector3Int(x, y, z));
        return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInChunkCoordinates);

    }

    private void GenerateVoxels(ChunkData chunkData)
    {
        for(int x = 0; x < chunkData.chunkSize; x++)
        {
            for(int z = 0; z < chunkData.chunkHeight; z++)
            {
                float noiseValue = Mathf.PerlinNoise((chunkData.worldPosition.x + x) * 0.01f, (chunkData.worldPosition.z + z) * 0.01f);

                int groundPosition = Mathf.RoundToInt(noiseValue * chunkHeight);

                for(int y = 0; y < chunkHeight; y++)
                {
                    BlockType voxelType = BlockType.Dirt;

                    if(y > groundPosition)
                    {
                        if(y < 10)
                        {
                            voxelType = BlockType.Water;
                        }
                        else
                        {
                            voxelType = BlockType.Air;
                        }
                    }
                    else if(y == groundPosition)
                    {
                        voxelType = BlockType.Grass;
                    }
                    Chunk.SetBlock(chunkData, new Vector3Int(x, y, z), voxelType);
                }
            }
        }
    }

    private void GenerateVoxel3D(ChunkData chunkData)
    {
        float[,,] surfaceLevels = new float[chunkSize,chunkHeight,chunkSize];
        for (int x = 0; x < chunkData.chunkSize; x++)
        {
            for (int z = 0; z < chunkData.chunkSize; z++)
            {

                for (int y = 0; y < chunkHeight; y++)
                {
                    var state = GetBlockState3D(x, y, z, chunkData);
                    surfaceLevels[x, y, z] = state.surfaceLevel;
                    if (state.solid)
                        Chunk.SetBlock(chunkData, new Vector3Int(x, y, z), BlockType.Sand);
                    else if (y > seaLevel) Chunk.SetBlock(chunkData, new Vector3Int(x, y, z), BlockType.Air);
                    else Chunk.SetBlock(chunkData, new Vector3Int(x, y, z), BlockType.Water);
                }
            }
        }

        for (int x = 0; x < chunkData.chunkSize; x++)
        {
            for (int z = 0; z < chunkData.chunkSize; z++)
            {
                bool grassPlaced = false;

                for (int y = 0; y < chunkHeight; y++)
                {
                    BlockType blockToSet = BlockType.Sand;
                    BlockType block = Chunk.GetBlockFromChunkCoordinates(chunkData, x, y, z);
                    BlockType upperBlock = y < chunkHeight ? Chunk.GetBlockFromChunkCoordinates(chunkData, x, y + 1, z) : BlockType.None;
                    blockToSet = block;
                    if (y >= seaLevel)
                    {
                        if(block == BlockType.Sand)
                        {
                            if (upperBlock == BlockType.Air || upperBlock == BlockType.None)
                            {
                                if (!grassPlaced)
                                {
                                    blockToSet = BlockType.Grass;
                                }
                                else blockToSet = BlockType.Stone;
                            } else
                            {
                                blockToSet = BlockType.Dirt;
                            }
                        }
                        else if(block != BlockType.Water)
                        {
                            blockToSet = BlockType.Air;
                        }
                    }
                    Chunk.SetBlock(chunkData, new Vector3Int(x, y, z), blockToSet);
                }
            }
        }
    }

    private float GetBlockState(int x, int y, int z, ChunkData chunkData)
    {
        float continentNoise = Noise.OctavePerlin(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, ContinentNoise);
        float erosionNoise = Noise.OctavePerlin(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, ErosionNoise);
        float pvNoise = Noise.OctavePerlin(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, PVNoise);

        float groundLevel = ContinentSpline.Evaluate(continentNoise) + ErosionSpline.Evaluate(erosionNoise) + PeaksAndValliesSpline.Evaluate(pvNoise); ;
        float surfaceLevel = groundLevel / 3f;
        return surfaceLevel;
    }

    private BlockState GetBlockState3D(int x, int y, int z, ChunkData chunkData)
    {
        float surfaceLevel = GetBlockState(x, y, z, chunkData);
        float noise = Noise.OctaveSimplex3D(x + chunkData.worldPosition.x, y + chunkData.worldPosition.y, z + chunkData.worldPosition.z, simplexNoise);

        noise = Noise.RemapValue(noise, 0, 256, -1, 1);
        float lerp = Mathf.InverseLerp(0, surfaceLevel * 2 > chunkHeight ? chunkHeight : surfaceLevel * 2, y);
        float fallOff = Mathf.Lerp(1, -1, lerp);
        float squash = Noise.OctaveSimplex3D(chunkData.worldPosition.x + x, chunkData.worldPosition.y + y, chunkData.worldPosition.z + z, squashNoise);
        squash = Noise.RemapValue(squash, 0, 256, 0, 1);
        noise += fallOff * squash * squashingFactor;

        var state = new BlockState(noise > 0, surfaceLevel);
        return state;
    }

    private struct BlockState
    {
        public bool solid;
        public float surfaceLevel;

        public BlockState(bool solid, float surfaceLevel)
        {
            this.solid = solid;
            this.surfaceLevel = surfaceLevel;
        }
    }
}