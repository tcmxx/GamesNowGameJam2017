using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour {


    public GameObject invisibleWallPref;


    public static TilesController tilesController;
    private BasicTile[,] tilesArray;
    private bool[,] path;

    private LevelGenerationData currentGenerationData;

    private void Awake()
    {
        tilesController = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GenerateTilesData(LevelGenerationData parameters)
    {
        currentGenerationData = parameters;
        //create the 2d arrays
        tilesArray = new BasicTile[currentGenerationData.sizeWidth, currentGenerationData.sizeLength];
        

        //generation the tiles
        float totalWeight = 0;
        foreach(var t in currentGenerationData.tileItems)
        {
            totalWeight += t.tileWeight;
        }

        for(int i = 0; i < currentGenerationData.sizeWidth; ++i)
        {
            for(int j = 0; j < currentGenerationData.sizeLength; ++j)
            {
                GameObject newTile = InstantiateRandomTileBasedOnData(totalWeight, 
                    Vector3.right*(currentGenerationData.tileWidth*(i + 0.5f))+
                    Vector3.forward*(currentGenerationData.tileLength*(j + 0.5f)));

                tilesArray[i, j] = newTile.GetComponent<BasicTile>();
            }
        }


        //add invisiblewalls
        for (int i = 0; i <= 1; ++i)
        {
            for (int j = 0; j < currentGenerationData.sizeLength; ++j)
            {
                Vector3 pos = Vector3.right * (currentGenerationData.tileWidth * (i * (currentGenerationData.sizeWidth + 1) - 0.5f)) +
                    Vector3.forward * (currentGenerationData.tileLength * (j + 0.5f));
                GameObject newTile = GameObject.Instantiate(invisibleWallPref, pos, Quaternion.identity, this.transform);
            }
        }
        for (int i = 0; i < currentGenerationData.sizeWidth; ++i)
        {
            for (int j = 0; j <= 1; ++j)
            {
                Vector3 pos = Vector3.right * (currentGenerationData.tileWidth * (i + 0.5f)) +
                    Vector3.forward * (currentGenerationData.tileLength * (j * (currentGenerationData.sizeLength + 1) - 0.5f));
                GameObject newTile = GameObject.Instantiate(invisibleWallPref, pos, Quaternion.identity, this.transform);
            }
        }
        
    }


    //mark a path with true. The generator will guarantee that this path is walkable
    private void GeneratePossiblePath(LevelGenerationData parameters)
    {
        path = new bool[currentGenerationData.sizeWidth, currentGenerationData.sizeLength];
        for(int i = 0; i < currentGenerationData.sizeWidth; ++i)
        {
            for(int j = 0; j < currentGenerationData.sizeLength; ++j)
            {
                path[i, j] = false;
            }
        }


        int place = Random.Range(0, currentGenerationData.sizeWidth);
        int prevPlace = place;
        for (int i = currentGenerationData.sizeLength-2; i >= 0; --i)
        {
            int place = Random.Range(0, currentGenerationData.sizeWidth);
        }
    }


    private GameObject InstantiateRandomTileBasedOnData(float totalWeight, Vector3 position)
    {
        float num = Random.Range(0, totalWeight);
        float currentWeight = 0;
        foreach(var i in currentGenerationData.tileItems)
        {
            currentWeight += i.tileWeight;
            if(currentWeight >= num)
            {
                return GameObject.Instantiate(i.tilePref, position, Quaternion.identity,this.transform);
            }
        }

        return null;
    }


    public BasicTile GetOnTile(Vector3 position)
    {
        int width = Mathf.FloorToInt(position.x / currentGenerationData.tileWidth);
        int height = Mathf.FloorToInt(position.z / currentGenerationData.tileLength);

        if(width < 0 || width >= currentGenerationData.sizeWidth || height < 0 || height >= currentGenerationData.sizeLength)
        {
            return null;
        }
        return  tilesArray[width, height];
    }

}
