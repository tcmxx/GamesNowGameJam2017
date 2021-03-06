﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public static MusicController musicController;

    private void Awake()
    {
        musicController = this;
    }

    // Use this for initialization
    void Start () {
        AkSoundEngine.PostEvent("Play_Music", gameObject);
        AkSoundEngine.PostEvent("Play_Menu_Music", gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        AkSoundEngine.SetRTPCValue("lifebar",CharacterControl.mainCharacter.CurrentAge/GamePlayUI.gamePlayUI.maxAgeBar*100,gameObject);
	}

    private void OnDisable()
    {
        StopMusic();
        AkSoundEngine.PostEvent("Stop_Level_End", gameObject); 
    }

    public void StopMusic()
    {
        AkSoundEngine.PostEvent("Stop_Music", gameObject);
        AkSoundEngine.PostEvent("Stop_Menu_music", gameObject);
        
    }
}
