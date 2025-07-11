using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class ChunkData
{
    public BlockType[] blocks;
    public int chunkSize = 16;
    public int chunkHeight = 100;
    public Vector3Int worldPosition;

    public World world;

    public bool modifiedByPlayer = false;

    public ChunkData(int chunkSize, int chunkHeight, Vector3Int worldPosition, World world)
    {
        this.chunkSize = chunkSize;
        this.chunkHeight = chunkHeight;
        this.worldPosition = worldPosition;
        this.world = world;

        blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
    }
}
