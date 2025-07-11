using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceLayerHandler : BlockLayerHandler
{
    public BlockType surfaceBlockType;
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight, int waterHeightThreshold, Vector3Int seedOffset)
    {
        if (y == surfaceHeight)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, surfaceBlockType);
            return true;
        }

        return false;
    }
}
