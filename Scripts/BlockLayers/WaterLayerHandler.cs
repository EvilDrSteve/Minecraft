using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLayerHandler : BlockLayerHandler
{
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight, int waterHeightThreshold, Vector3Int seedOffset)
    {
        if(y > surfaceHeight && y < waterHeightThreshold)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, BlockType.Water);

            if(y == surfaceHeight + 1)
            {
                pos.y = surfaceHeight;
                Chunk.SetBlock(chunkData, pos, BlockType.Sand);
            }

            return true;
        }

        return false;
    }
}
