using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PowerupBasic {



    public override void OnPlayerObtained(CharacterControl character)
    {
        base.OnPlayerObtained(character);
        GameController.gameController.PassLevelLogic();
    }


    
}
