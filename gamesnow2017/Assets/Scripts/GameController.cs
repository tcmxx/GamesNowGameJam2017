﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public static GameController gameController;

    


    public float SavedAge { get { return savedAge; } set { savedAge = value; } }
    private float savedAge = 0;

    public int CurrentLevelNum { get; private set; }

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void RestartGame()
    {
        SavedAge = 0;
        CurrentLevelNum = 0;
        SceneTransitionHelper.Instance.StartLoadingScene("Level" + CurrentLevelNum);
        //SceneManager.LoadScene("Level" + CurrentLevelNum);
    }
    public void StartNextLevel()
    {
        if (HasNextScene())
        {
            SavedAge = 0;
            CurrentLevelNum++;
            SceneTransitionHelper.Instance.StartLoadingScene("Level" + CurrentLevelNum);
            //SceneManager.LoadScene("Level" + CurrentLevelNum);
        }
    }

    public bool HasNextScene()
    {
        int num = (CurrentLevelNum + 1);
        return Application.CanStreamedLevelBeLoaded("Level" + num);
    }

    public void PassLevelLogic()
    {
        CharacterControl.mainCharacter.PassLevel();
        GamePlayUI.gamePlayUI.ShowEndGameUI(true);

    }

    public void PlayeyDieLogic()
    {
        GamePlayUI.gamePlayUI.ShowEndGameUI(false);
    }
    
    public void ChangeCharacter(float newAge)
    {
        SavedAge = newAge;

        CharacterControl prevCharacter = CharacterControl.mainCharacter;

        GameObject newCharacter = Instantiate(CharacterPresets.characterPresets.GetCharacterPref(newAge), prevCharacter.transform.position, prevCharacter.transform.rotation);
        CharacterControl newControl = newCharacter.GetComponent<CharacterControl>();
        newControl.CurrentAge = SavedAge;
        newControl.BasicCharactor.CurrentVelocity = prevCharacter.BasicCharactor.CurrentVelocity;
        Destroy(prevCharacter.gameObject);

        AkSoundEngine.PostEvent("Play_Player_Gets_Older", gameObject);
        Debug.Log("Character Changed");
    }
}
