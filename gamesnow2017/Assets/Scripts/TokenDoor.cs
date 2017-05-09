using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenDoor : TokenBasic {



    public override void OnPlayerObtained(CharacterControl character)
    {
        base.OnPlayerObtained(character);
        character.PassLevel();
        GameController.gameController.PassLevelLogic();
    }


    
}
