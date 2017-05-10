using System.Collections;
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
        SceneManager.LoadScene("Level" + CurrentLevelNum);
    }
    public void StartNextLevel()
    {
        if (HasNextScene())
        {
            SavedAge = 0;
            CurrentLevelNum++;
            SceneManager.LoadScene("Level" + CurrentLevelNum);
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
        GamePlayUI.gamePlayUI.ShowEndGameUI();

    }
    
    public void ChangeCharacter(float newAge)
    {
        SavedAge = newAge;

        CharacterControl prevCharacter = CharacterControl.mainCharacter;

        GameObject newCharacter = Instantiate(CharacterPresets.characterPresets.GetCharacterPref(newAge), prevCharacter.transform.position, prevCharacter.transform.rotation);
        newCharacter.GetComponent<CharacterControl>().CurrentAge = SavedAge;
        Destroy(prevCharacter.gameObject);

        Debug.Log("Character Changed");
    }
}
