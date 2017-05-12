using System.Collections;
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
    public List<BasicTile> CanStandOnTiles { get; private set; }


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
        //create the containers
        tilesArray = new BasicTile[currentGenerationData.sizeWidth, currentGenerationData.sizeLength];
        CanStandOnTiles = new List<BasicTile>();
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
                    CanStandOnTiles.Add(tilesArray[i, j]);
                }
            }
        }

        //generate the powerups
        InstantiatePowerupsOnStandableTiles();

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
                BasicTile tile = result.GetComponent<BasicTile>();
                if (allowBlock == false && tile.canStandOn == false)
                {
                    //not allow blocked tile ant it is a block tile.
                    Destroy(result);
                    result = Instantiate(planeTilePref, position, Quaternion.identity, this.transform);
                }

                if (tile.symmetric)
                {
                    result.transform.localEulerAngles = Vector3.up*((int)(Random.Range(0, 4.0f))) * 90;
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

    private void InstantiatePowerupsOnStandableTiles()
    {
        List<BasicTile> copiedList = new List<BasicTile>();
        copiedList.AddRange(CanStandOnTiles);

        foreach (var pu in currentGenerationData.powerupItems)
        {
            int num = Random.Range(pu.minNumber, pu.maxNumber + 1);
            for(int i = 0; i < num; ++i)
            {
                int index = Random.Range(0, copiedList.Count);
                BasicTile tile = copiedList[index];
                Instantiate(pu.pref, tile.transform.position + Vector3.up * 0.3f, Quaternion.identity, transform);
                copiedList.RemoveAt(index);
                if(copiedList.Count <= 0)
                {
                    return;
                }
            }
        }
    }
    
}
