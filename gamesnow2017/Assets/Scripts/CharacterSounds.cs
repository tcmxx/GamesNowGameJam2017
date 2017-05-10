using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

    public  string footstepSound;
    public GameObject obj;


    public void PlayFootStep()
    {
        AkSoundEngine.PostEvent(footstepSound, obj);
    }
}
