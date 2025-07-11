using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static float textureOffset = 0.000f;
    public static float textureSizeX, textureSizeY;
    public static float waterTextureSizeX = 1, waterTextureSizeY = 1;
    public static Dictionary<BlockType, TextureData> blockTextureDictionary = new Dictionary<BlockType, TextureData>();
    public BlockData textureData;

    private void Awake()
    {
        foreach(var item in textureData.textureDataList)
        {
            if (blockTextureDictionary.ContainsKey(item.type) == false)
            {
                blockTextureDictionary.Add(item.type, item);
            }
        }

        textureSizeX = textureData.textureSizeX;
        textureSizeY = textureData.textureSizeY;
    }
}
