using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AkSoundEngine.PostEvent("Play_Menu_Music", gameObject);
        AkSoundEngine.SetRTPCValue("lifebar", 0, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.Log("Reselecting first input");
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
    }
    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("Stop_Menu_music", gameObject);
    }
    public void StartButtonClicked()
    {
        AkSoundEngine.PostEvent("Play_Button", gameObject);
        GameController.gameController.RestartGame(true);
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
