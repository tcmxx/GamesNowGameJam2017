using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationController : MonoBehaviour {

    public GameObject doorPref;


	// Use this for initialization
	void Start () {
        InitializeLevel();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitializeLevel()
    {
        TilesController.tilesController.GenerateTilesData(LevelGenerationData.levelGenerationData);

        //create the character 
        Instantiate(CharacterPresets.characterPresets.GetCharacterPref(GameController.gameController.SavedAge),
            TilesController.tilesController.CharactorStartPosition + Vector3.up*1,Quaternion.identity
            );

        Instantiate(doorPref, TilesController.tilesController.DestinationPosition + Vector3.up * 0.5f, Quaternion.identity);
        
    }
    

}
