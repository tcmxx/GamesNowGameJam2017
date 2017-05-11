﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour {


    public GameObject invisibleWallPref;
    public GameObject planeTilePref;

    public static TilesController tilesController;
    private BasicTile[,] tilesArray;
    private bool[,] path;

    private LevelGenerationData currentGenerationData;

    public Vector3 CharactorStartPosition { get; set; }
    public Vector3 DestinationPosition { get; set; }

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
        //generate a guarranteed path
        GeneratePossiblePath(currentGenerationData);

        //generation the tiles
        float totalWeight = 0;
        foreach(var t in currentGenerationData.tileItems)
        {
            totalWeight += t.data;
        }

        for(int i = 0; i < currentGenerationData.sizeWidth; ++i)
        {
            for(int j = 0; j < currentGenerationData.sizeLength; ++j)
            {
                //bool whether
                GameObject newTile = InstantiateRandomTileBasedOnData(totalWeight, 
                    currentGenerationData.GridToWorldPosition(i,j), !path[i,j]);

                tilesArray[i, j] = newTile.GetComponent<BasicTile>();

                if (tilesArray[i, j].canStandOn)
                {
                    RandomInstantiatePowerUp(currentGenerationData.GridToWorldPosition(i, j) + Vector3.up*0.35f);
                }
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
        DestinationPosition = currentGenerationData.GridToWorldPosition(place, currentGenerationData.sizeLength - 1);

        int prevPlace = place;
        path[place, currentGenerationData.sizeLength - 1] = true;
        bool toggle = true;
        for (int i = currentGenerationData.sizeLength-2; i >= 0; --i)
        {
            if (toggle)
            {
                toggle = false;
                path[prevPlace, i] = true;
            }
            else
            {
                toggle = true;
                place = Random.Range(0, currentGenerationData.sizeWidth);
                for (int j = Mathf.Min(place, prevPlace); j <= Mathf.Max(place,prevPlace); ++j)
                {
                    path[j, i] = true;
                }
            }
            prevPlace = place;
        }

        CharactorStartPosition = currentGenerationData.GridToWorldPosition(place, 0);
    }


    private GameObject InstantiateRandomTileBasedOnData(float totalWeight, Vector3 position, bool allowBlock = true)
    {
        GameObject result = null;

        float num = Random.Range(0, totalWeight);
        float currentWeight = 0;
        foreach (var i in currentGenerationData.tileItems)
        {
            currentWeight += i.data;
            if (currentWeight >= num)
            {
                result = Instantiate(i.pref, position, Quaternion.identity, this.transform);
                if (allowBlock == false && result.GetComponent<BasicTile>().canStandOn == false)
                {
                    //not allow blocked tile ant it is a block tile.
                    Destroy(result);
                    result = Instantiate(planeTilePref, position, Quaternion.identity, this.transform);
                }
                break;
            }
        }
        

        return result;
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

    private GameObject RandomInstantiatePowerUp(Vector3 position)
    {
        GameObject result = null;
        float num = Random.Range(0, 1.0f);
        float currentChance = 0;
        foreach (var i in currentGenerationData.powerupItems)
        {
            currentChance += i.data;
            if (currentChance >= num)
            {
                result = Instantiate(i.pref, position, Quaternion.identity, this.transform);
                break;
            }
        }
        return result;
    }
    
}
