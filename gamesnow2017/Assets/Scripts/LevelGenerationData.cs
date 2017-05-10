using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationData : MonoBehaviour {

    public static LevelGenerationData levelGenerationData;
    
    [System.Serializable]
    public struct TileItem
    {
        public GameObject tilePref;
        public float tileWeight;    //chance of this tile
    }

    public int sizeWidth = 8;       //size of the scene.
    public int sizeLength = 40;

    public float tileWidth = 1;
    public float tileLength = 1;
    //tiles
    public TileItem[] tileItems;


    private void Awake()
    {
        levelGenerationData = this;
    }

    public Vector3 GridToWorldPosition(int x, int y)
    {
        return Vector3.right * (tileWidth * (x + 0.5f)) +
                    Vector3.forward * (tileLength * (y + 0.5f));
    }
}
