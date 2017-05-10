using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileClosed : BasicTile {


    public override void OnCharacterCollisionEnter(CharacterControl charactor)
    {
        if (charactor.BasicCharactor.Status == CharactorMovement.CharacterStatus.Jumping)
        {
            Vector3 tempV = charactor.BasicCharactor.CurrentVelocity;
            Vector3 towards = transform.position - charactor.transform.position;
            towards.y = 0;

            Vector3 newV = tempV - Vector3.Dot(tempV, towards.normalized) * towards.normalized;
            newV = newV.normalized * tempV.magnitude;
            charactor.BasicCharactor.CurrentVelocity = newV;
        }
    }

}
