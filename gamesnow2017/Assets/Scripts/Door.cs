using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PowerupBasic {



    public override void OnUsePowerup(CharacterControl character)
    {
        base.OnUsePowerup(character);
        GameController.gameController.PassLevelLogic();
    }


    
}
