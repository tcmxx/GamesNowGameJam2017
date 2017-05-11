using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    public void StartButtonClicked()
    {
        AkSoundEngine.PostEvent("Play_Button", gameObject);
        GameController.gameController.RestartGame();
    }

    public void QuitButtonClicked()
    {
        AkSoundEngine.PostEvent("Play_Button", gameObject);
        Application.Quit();
    }

    public void PlayButtonSound()
    {
        AkSoundEngine.PostEvent("Play_Button", gameObject);
    }
}
