using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GamePlayUI : MonoBehaviour {

    public static GamePlayUI gamePlayUI;
    [Header("References")]
    public Image ageBar;
    public Text ageText;
    public GameObject passGameUI;
    public GameObject loseGameUI;
    public GameObject continueButton;
    [Header("Paramters")]
    public float maxAgeBar = 100f;


    private void Awake()
    {
        gamePlayUI = this; 
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdageAgeInfo();

    }


    public void ShowEndGameUI(bool pass)
    {
        if (pass)
        {
            passGameUI.SetActive(true);
            if (!GameController.gameController.HasNextScene())
            {
                continueButton.SetActive(false);
            }
            else
            {
                continueButton.SetActive(true);
            }
        }else
        {
            loseGameUI.SetActive(true);
        }
    }

    public void OnContinueButtonClicked()
    {
        GameController.gameController.StartNextLevel();
        AkSoundEngine.PostEvent("Play_Button", gameObject);
    }

    public void OnQuitClicked()
    {
        SceneManager.LoadScene("MenuScene");
        AkSoundEngine.PostEvent("Play_Button", gameObject);
    }

    public void OnRestartClicked()
    {
        GameController.gameController.RestartGame();
        AkSoundEngine.PostEvent("Play_Button", gameObject);
    }

    private void UpdageAgeInfo()
    {
        ageBar.fillAmount = CharacterControl.mainCharacter.CurrentAge/maxAgeBar;
        ageText.text = "Age: " + CharacterControl.mainCharacter.CurrentAge.ToString("F0");
    }
}
