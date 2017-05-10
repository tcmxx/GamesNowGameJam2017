using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileWall : BasicTile {
    
    public override void OnCharacterCollisionEnter(CharacterControl charactor)
    {
        if(charactor == inTileCharacter || charactor.BasicCharactor.Status == CharactorMovement.CharacterStatus.Jumping)
            charactor.StartJumping();
    }
    public override void OnCharacterCollisionExit(CharacterControl charactor)
    {
        charactor.EndJumping();
    }
}
