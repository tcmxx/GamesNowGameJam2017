using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AkSoundEngine.PostEvent("Play_Music", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        AkSoundEngine.SetRTPCValue("lifebar",CharacterControl.mainCharacter.CurrentAge/GamePlayUI.gamePlayUI.maxAgeBar*100,gameObject);
	}

    private void OnDisable()
    {
        AkSoundEngine.PostEvent("Stop_Music", gameObject);
    }
}
