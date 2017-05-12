using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public static GameController gameController;




    public float SavedAge { get { return savedAge; } set { savedAge = value; } }
    private float savedAge = 0;

    public float LevelCostTime { get; private set; }
    private bool paused = true;

    public int CurrentLevelNum { get; private set; }

    private PowerupBasic powerup1 = null;
    private PowerupBasic powerup2 = null;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        if(!paused)
            LevelCostTime += Time.deltaTime;
    }


    public void RestartGame(bool fromBeginning = false)
    {
        SavedAge = 0;
        LevelCostTime = 0;
        paused = false;
        if (fromBeginning)
            CurrentLevelNum = 0;
        SceneTransitionHelper.Instance.StartLoadingScene("Level" + CurrentLevelNum);
    }
    public void StartNextLevel()
    {
        if (HasNextScene())
        {
            paused = false;
            SavedAge = 0;
            LevelCostTime = 0;
            CurrentLevelNum++;
            SceneTransitionHelper.Instance.StartLoadingScene("Level" + CurrentLevelNum);
        }
    }

    public bool HasNextScene()
    {
        int num = (CurrentLevelNum + 1);
        return Application.CanStreamedLevelBeLoaded("Level" + num);
    }

    public void PassLevelLogic()
    {
        paused = true;
        CharacterControl.mainCharacter.PassLevel();
        GamePlayUI.gamePlayUI.ShowEndGameUI(true);
        MusicController.musicController.StopMusic();

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


    public void ObtainPowerUp(PowerupBasic powerup)
    {
        powerup.transform.position = Vector3.back * 1000;
        if(powerup1 == null)
        {
            powerup1 = powerup;
        }else if(powerup2 == null)
        {
            powerup2 = powerup;
        }else
        {
            Destroy(powerup1.gameObject);
            powerup1 = powerup2;
            powerup2 = powerup;
        }

        GamePlayUI.gamePlayUI.SetPowerupImages(powerup1, powerup2);
    }

    public void UseNextPowerup()
    {
        if(powerup1 != null)
        {
            powerup1.OnUsePowerup(CharacterControl.mainCharacter);
            powerup1 = powerup2;
            powerup2 = null;
        }

        GamePlayUI.gamePlayUI.SetPowerupImages(powerup1, powerup2);
    }
}
