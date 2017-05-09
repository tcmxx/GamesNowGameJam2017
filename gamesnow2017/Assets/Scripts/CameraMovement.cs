using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FollowPlayer(CharacterControl.mainCharacter);

    }



    private void FollowPlayer(CharacterControl character)
    {
        Vector3 pos = transform.position;
        pos.z = character.transform.position.z - 5;

        transform.position = pos;
    }
}
