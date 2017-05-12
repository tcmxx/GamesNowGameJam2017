using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GamePlayUI : MonoBehaviour {

    public static GamePlayUI gamePlayUI;
    [Header("References")]
    public Image ageBar;
    public Text ageText;
    public GameObject passGameUI;
    public GameObject loseGameUI;
    public Button continueButton;
    public Button restartButton;
    public Button passGameQuitButton;
    public Image powerup1Image;
    public Image powerup2Image;
    public Sprite noPowerupSprite;
    [Header("Paramters")]
    public float maxAgeBar = 100f;


    [System.Serializable]
    public struct IconColor
    {
        public Image icon;
        public AgeStage[] ageStage;
        public Color[] color;
    }
    public IconColor[] iconColors;

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
        ChangeIconsColor(CharacterControl.mainCharacter.CurrentStage);
    }


    public void ShowEndGameUI(bool pass)
    {
        if (pass)
        {
            passGameUI.SetActive(true);
            if (!GameController.gameController.HasNextScene())
            {
                continueButton.gameObject.SetActive(false);
                passGameQuitButton.Select();
            }
            else
            {
                continueButton.gameObject.SetActive(true);
                continueButton.Select();
            }
        }else
        {
            loseGameUI.SetActive(true);
            restartButton.Select();
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


    public void ChangeIconsColor(AgeStage ageStage)
    {
        foreach(var icon in iconColors)
        {
            Color col = Color.white;
            for(int i = 0; i < icon.ageStage.Length; ++i)
            {
                if(icon.ageStage[i] == ageStage)
                {
                    col = icon.color[i];
                }
            }

            icon.icon.color = col;
        }
    }

    public void SetPowerupImages(PowerupBasic powerup1, PowerupBasic powerup2)
    {
        if(powerup1 != null)
        {
            powerup1Image.sprite = powerup1.powerUpImage;
        }else
        {
            powerup1Image.sprite = noPowerupSprite;
        }

        if(powerup2 != null)
        {
            powerup2Image.sprite = powerup2.powerUpImage;
        }else
        {
            powerup2Image.sprite = noPowerupSprite;
        }
    }
}
