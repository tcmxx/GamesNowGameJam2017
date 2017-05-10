using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresets : MonoBehaviour {

    public static CharacterPresets characterPresets;

    [System.Serializable]
    public struct AgeStageSetting
    {
        public AgeStage stage;
        public float maxAge;
        public GameObject characterPref;
    }

    public AgeStageSetting[] ageStageSetting;


    private void Awake()
    {
        characterPresets = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AgeStage CheckStageForAge(float age)
    {
        foreach (var s in ageStageSetting)
        {
            if (age < s.maxAge)
            {
                return s.stage;
            }
        }

        return AgeStage.Kid;
    }


    public GameObject GetCharacterPref(float age)
    {
        return GetCharacterPref(CheckStageForAge(age));
    }
    public GameObject GetCharacterPref(AgeStage ageStage)
    {
        foreach (var c in ageStageSetting)
        {
            if (c.stage == ageStage)
            {
                return c.characterPref;
            }
        }
        return null;
    }
}


public enum AgeStage
{
    Kid,
    Teen,
    Adult,
    Old,
    Death
}