using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public static GameController gameController;

    


    public float SavedAge { get { return savedAge; } set { savedAge = value; } }
    private float savedAge = 0;

    void Awake()
    {
        if (gameController == null)
        {
            Debug.Log("GameController doesn't exist yet. Created new one.");
            DontDestroyOnLoad(gameObject);
            gameController = this;
        }
        else if (gameController != this)
        {
            Debug.Log("There is more than one GameController. Destroyed the new one.");
            Destroy(gameObject);
        }


    }

    // Use this for initialization
    void Start () {
        TilesController.tilesController.GenerateTilesData(LevelGenerationData.levelGenerationData);
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void PassLevelLogic()
    {

    }
}
