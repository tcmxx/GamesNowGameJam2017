using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

    public  string footstepSound;
    public GameObject obj;

    private void Start()
    {
        AkSoundEngine.SetSwitch("material", "concrete", gameObject);
    }

    public void PlayFootStep()
    {
        AkSoundEngine.PostEvent(footstepSound, obj);
    }
}
