using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

    public  string footstepSound;
    public GameObject obj;

    private void Start()
    {
        AkSoundEngine.SetSwitch("material", "concrete", MusicController.musicController.gameObject);
    }

    public void PlayFootStep()
    {
        AkSoundEngine.PostEvent(footstepSound, MusicController.musicController.gameObject);
        AkSoundEngine.SetRTPCValue("lifebar", CharacterControl.mainCharacter.CurrentAge / GamePlayUI.gamePlayUI.maxAgeBar * 100, MusicController.musicController.gameObject);
    }
}
