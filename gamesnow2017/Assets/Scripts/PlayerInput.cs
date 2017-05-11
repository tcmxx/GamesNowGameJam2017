using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControl))]
public class PlayerInput : MonoBehaviour {


    private CharacterControl charactorControl;



    private void Awake()
    {
        charactorControl = GetComponent<CharacterControl>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        charactorControl.MoveHorizontal(Input.GetAxis("Horizontal"));
        charactorControl.MoveVertical(Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("UsePowerup"))
        {
            GameController.gameController.UseNextPowerup();
        }
	}
}
