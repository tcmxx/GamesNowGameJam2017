using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerupAge : PowerupBasic
{

    public float changeOfAgeTime = 0;

    public override void OnUsePowerup(CharacterControl character)
    {
        base.OnUsePowerup(character);

        AgeStage stageOld = character.CurrentStage;

        character.CurrentAge += changeOfAgeTime*LevelGenerationData.levelGenerationData.agingSpeed;
        if(character.CurrentAge <= 0)
        {
            character.CurrentAge = 0;
        }
        
        if (stageOld != character.CurrentStage)
        {
            GameController.gameController.ChangeCharacter(character.CurrentAge);
        }

        ApplyAsBuff(0.5f, character);
    }
}