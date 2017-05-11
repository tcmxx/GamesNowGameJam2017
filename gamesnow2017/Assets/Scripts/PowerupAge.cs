using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerupAge : PowerupBasic
{

    public float changeOfAge = 0;

    public override void OnPlayerObtained(CharacterControl character)
    {
        base.OnPlayerObtained(character);

        AgeStage stageOld = character.CurrentStage;

        character.CurrentAge += changeOfAge;
        if(character.CurrentAge <= 0)
        {
            character.CurrentAge = 0;
        }
        
        if (stageOld != character.CurrentStage)
        {
            GameController.gameController.ChangeCharacter(character.CurrentAge);
        }

        Destroy(gameObject);
    }
}