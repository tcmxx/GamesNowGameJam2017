using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed : PowerupBasic {
    public int buffLastTime = 2;
    public float changeOfAcceleration = 0;
    public float chageOfDeceleration = 0;
    public float changeOfMaxSpeed = 0;
    public float changeOfAge = 0;

    public override void OnPlayerObtained(CharacterControl character)
    {
        base.OnPlayerObtained(character);

        character.CurrentAge += changeOfAge;
        if (character.CurrentAge <= 0)
        {
            character.CurrentAge = 0;
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
