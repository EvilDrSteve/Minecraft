using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLayerHandler : BlockLayerHandler
{
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight,  int waterHeightThreshold, Vector3Int seedOffset)
    {
        if (y > surfaceHeight)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, BlockType.Air);
            return true;
        }

        return false;
    }
}
