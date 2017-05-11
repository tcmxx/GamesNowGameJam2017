using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed : PowerupBasic {
    public int buffLastTime = 2;
    public float changeOfAcceleration = 0;
    public float chageOfDeceleration = 0;
    public float changeOfMaxSpeed = 0;
    public float changeOfAgeTime = 0;

    public override void OnUsePowerup(CharacterControl character)
    {
        base.OnUsePowerup(character);

        AgeStage stageOld = character.CurrentStage;

        character.CurrentAge += changeOfAgeTime * LevelGenerationData.levelGenerationData.agingSpeed;
        if (character.CurrentAge <= 0)
        {
            character.CurrentAge = 0;
        }

        if (stageOld != character.CurrentStage)
        {
            GameController.gameController.ChangeCharacter(character.CurrentAge);
        }

        ApplyAsBuff(buffLastTime, character);
    }

    public override void OnBuffApplied()
    {
        effectedCharacter.BasicCharactor.acceleration += changeOfAcceleration;
        effectedCharacter.BasicCharactor.deceleration += chageOfDeceleration;
        effectedCharacter.maxSpeed += changeOfMaxSpeed;
    }

    public override void OnBuffRemoved()
    {
        effectedCharacter.BasicCharactor.acceleration -= changeOfAcceleration;
        effectedCharacter.BasicCharactor.deceleration -= chageOfDeceleration;
        effectedCharacter.maxSpeed -= changeOfMaxSpeed;
    }
}
