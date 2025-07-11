using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockLayerHandler : MonoBehaviour
{
    [SerializeField]
    private BlockLayerHandler Next;

    public bool Handle(ChunkData chunkData, int x, int y, int z, int surfaceHeight, int waterHeight, Vector3Int seedOffset)
    {
        if (TryHandling(chunkData, x, y, z, surfaceHeight, waterHeight, seedOffset))
            return true;
        else if(Next != null)
        {
            return Next.Handle(chunkData, x, y, z, surfaceHeight, waterHeight, seedOffset);
        }

        return false;
    }

    protected abstract bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeight, int waterHeight, Vector3Int seedOffset);
}
