using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BasicTile : MonoBehaviour {

    //whether this object can be standed on. used for the map generator to put objects
    public bool canStandOn = true;
    public string wwiseSwitch = "";
    public StageBasedParamter[] stageBasedParamters;


    [System.Serializable]
    public struct StageBasedParamter
    {
        public AgeStage stage;
        public float speedOffset;
        public float speedMultiplier;
        public float accelerationOffset;
        public float decelerationOffset;
    }


    public void Update()
    {
        
    }


    public virtual void OnEnterTile(CharacterControl charactor)
    {
        Debug.Log("Enter Tile: " + gameObject.name);
        StageBasedParamter param = FindStageBasedParamters(charactor.CurrentStage);
        charactor.BasicCharactor.acceleration += param.accelerationOffset;
        charactor.BasicCharactor.deceleration += param.decelerationOffset;
        if (wwiseSwitch != "")
        {
            AkSoundEngine.SetSwitch("material", wwiseSwitch, charactor.gameObject);
        }
    }

    public virtual void OnExitTile(CharacterControl charactor)
    {
        Debug.Log("Exit Tile: " + gameObject.name);
        StageBasedParamter param = FindStageBasedParamters(charactor.CurrentStage);
        charactor.BasicCharactor.acceleration -= param.accelerationOffset;
        charactor.BasicCharactor.deceleration -= param.decelerationOffset;
    }

    public virtual float ModifySpeed(float inSpeed, CharacterControl charactor)
    {
        StageBasedParamter param = FindStageBasedParamters(charactor.CurrentStage);
        return inSpeed* param.speedMultiplier + param.speedOffset;
    }


    public StageBasedParamter FindStageBasedParamters(AgeStage stage)
    {
        foreach(var p in stageBasedParamters)
        {
            if(p.stage == stage)
            {
                return p;
            }
        }

        StageBasedParamter defaultResult = new StageBasedParamter();
        defaultResult.speedOffset = 0;
        defaultResult.speedMultiplier = 1;
        return defaultResult;
    }


}
