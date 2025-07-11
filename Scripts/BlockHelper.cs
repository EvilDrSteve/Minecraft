using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class BlockHelper
{

    private static Direction[] directions =
    {
        Direction.Forward,
        Direction.Backward,
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right
    };
    static Vector3[] vertices =
    {
        new Vector3(-1, -1, -1),
        new Vector3(-1, 1, -1),
        new Vector3(1, 1, -1),
        new Vector3(1, -1, -1),
        new Vector3(-1, -1, 1),
        new Vector3(-1, 1, 1),
        new Vector3(1, 1, 1),
        new Vector3(1, -1, 1),
    };

    static int[][] faceTriangles =
    {
        new int[] {0, 1, 2, 3},
        new int[] {7, 6, 5, 4},
        new int[] {1, 5, 6, 2},
        new int[] {4, 0, 3, 7},
        new int[] {4, 5, 1, 0},
        new int[] {3, 2, 6, 7},

    };

    static Vector3Int[] neighbourOffsets =
    {
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1,0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0),
    };
    public static Vector2Int TexturePosition(Direction direction, BlockType blockType)
    {
        return direction switch
        {
            Direction.Up => BlockDataManager.blockTextureDictionary[blockType].up,
            Direction.Down => BlockDataManager.blockTextureDictionary[blockType].down,
            _ => BlockDataManager.blockTextureDictionary[blockType].side
        };
    }

    public static Vector2[] GetFaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] Uvs = new Vector2[4];
        Vector2Int tilePos = TexturePosition(direction, blockType);
        float tSizeX = BlockDataManager.textureSizeX, tSizeY = BlockDataManager.textureSizeY, tOffset = BlockDataManager.textureOffset;
        if (blockType == BlockType.Water)
        {
            tSizeX = BlockDataManager.waterTextureSizeX;
            tSizeY = BlockDataManager.waterTextureSizeY;
        }
        Uvs[0] = new Vector2(tSizeX * tilePos.x + tSizeX - tOffset, tSizeY * tilePos.y + tOffset);
        Uvs[1] = new Vector2(tSizeX * tilePos.x + tSizeX - tOffset, tSizeY * tilePos.y + tSizeY - tOffset);
        Uvs[2] = new Vector2(tSizeX * tilePos.x + tOffset, tSizeY * tilePos.y + tSizeY - tOffset );
        Uvs[3] = new Vector2(tSizeX * tilePos.x + tOffset, tSizeY * tilePos.y + tOffset);

        return Uvs;
    }

    public static MeshData GetFaceIn(Direction direction, ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        GetFaceVertices(direction, x, y, z, meshData, blockType);
        meshData.AddTriangles(BlockDataManager.blockTextureDictionary[blockType].generatesCollider);
        meshData.uvs.AddRange(GetFaceUVs(direction, blockType));

        return meshData;
    }
    public static void GetFaceVertices(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        var generatesCollider = BlockDataManager.blockTextureDictionary[blockType].generatesCollider;

        switch (direction)
        {
            case Direction.Backward:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.Forward:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.Left:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;

            case Direction.Right:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.Down:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.Up:
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                break;
            default:
                break;
        }
    }

    public static MeshData GetMeshData(ChunkData chunkData, int x, int y, int z,MeshData meshData, BlockType blockType)
    {
        if (blockType == BlockType.Air || blockType == BlockType.None) return meshData;

        foreach(Direction direction in directions)
        {
            var neighbourBlockCoordinates = new Vector3Int(x, y, z) + direction.GetVector();
            var neighbourBlockType = Chunk.GetBlockFromChunkCoordinates(chunkData, neighbourBlockCoordinates);

            if (BlockDataManager.blockTextureDictionary[neighbourBlockType].isSolid == false)
            {

                if (blockType == BlockType.Water)
                {
                    if (neighbourBlockType == BlockType.Air)
                        meshData.waterMesh = GetFaceIn(direction, chunkData, x, y, z, meshData.waterMesh, blockType);
                }
                else
                {
                    meshData = GetFaceIn(direction, chunkData, x, y, z, meshData, blockType);
                }

            }
        }

        return meshData;
    }
}
