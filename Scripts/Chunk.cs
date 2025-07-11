using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Chunk
{

    private static void LoopThroughBlocks(ChunkData chunkData, Action<int, int, int> action)
    {
        for(int i = 0; i < chunkData.blocks.Length; i++)
        {
            var position = GetBlockPositionFromIndex(chunkData, i);
            action(position.x, position.y, position.z);
        }
    }

    public static void SetBlock(ChunkData chunkData, Vector3Int localPosition, BlockType block)
    {
        if(InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) && InRange(chunkData, localPosition.z))
        {
            int index = GetBlockIndexFromPosition(chunkData, localPosition.x, localPosition.y, localPosition.z);
            chunkData.blocks[index] = block;
        }
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        if (InRange(chunkData, x) && InRangeHeight(chunkData, y) && InRange(chunkData, z))
        {
            int index = GetBlockIndexFromPosition(chunkData, x, y, z);
            return chunkData.blocks[index];
        }

        return chunkData.world.GetBlockFromChunkCoordinates(chunkData, chunkData.worldPosition.x + x, chunkData.worldPosition.y + y, chunkData.worldPosition.z + z);
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int position)
    {
        return GetBlockFromChunkCoordinates(chunkData, position.x, position.y, position.z);

    }
    private static int GetBlockIndexFromPosition(ChunkData chunkData, int x, int y, int z)
    {
        return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
    }

    public static Vector3Int GetBlockChunkCoordinates(ChunkData chunkData, Vector3Int position)
    {
        return new Vector3Int
        {
            x = position.x - chunkData.worldPosition.x,
            y = position.y - chunkData.worldPosition.y,
            z = position.z - chunkData.worldPosition.z

        };
    }
    private static bool InRange(ChunkData chunkData, int axisCoordinate)
    {
        if (axisCoordinate < 0 || axisCoordinate >= chunkData.chunkSize) return false;

        return true;
    }

    private static bool InRangeHeight(ChunkData chunkData, int yCoordinate)
    {
        if (yCoordinate < 0 || yCoordinate >= chunkData.chunkHeight) return false;

        return true;
    }
    private static Vector3Int GetBlockPositionFromIndex(ChunkData chunkData, int index)
    {
        int x = index % chunkData.chunkSize;
        int y = (index / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = index / (chunkData.chunkSize * chunkData.chunkHeight);

        return new Vector3Int(x, y, z);

    }

    public static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new MeshData(true);

        LoopThroughBlocks(chunkData, (x, y, z) => meshData = BlockHelper.GetMeshData(chunkData, x, y, z, meshData, chunkData.blocks[GetBlockIndexFromPosition(chunkData, x, y, z)]));

        return meshData;
    }

    public static Vector3Int chunkPositionFromBlockCoords(World world, int x, int y, int z)
    {
        Vector3Int pos = new Vector3Int
        {
            x = Mathf.FloorToInt(x / (float)world.chunkSize) * world.chunkSize,
            y = Mathf.FloorToInt(y / (float)world.chunkHeight) * world.chunkHeight,
            z = Mathf.FloorToInt(z / (float)world.chunkSize) * world.chunkSize
        };

        return pos;
    }
}
