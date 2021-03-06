﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharactorMovement))]
public class CharacterControl : MonoBehaviour {


    public static CharacterControl mainCharacter;

    public float CurrentAge {
        get { return currentAge; }
        set {
            currentAge = value;
            CurrentStage = CharacterPresets.characterPresets.CheckStageForAge(currentAge);
            if(CurrentStage == AgeStage.Death)
            {
                Die();
            }
        }
    }
    [SerializeField]
    private float currentAge;

    public AgeStage CurrentStage { get; private set; }
    
    public float maxSpeed;



    private float horizontalInput;
    private float verticalInput;

    public CharactorMovement BasicCharactor { get; set; }
    public BasicTile CurrentTile { get; set; }



    private void Awake()
    {
        BasicCharactor = GetComponent<CharactorMovement>();
        mainCharacter = this;
        CurrentTile = null;
        
    }
    // Use this for initialization
    void Start()
    {
        CheckCurrentTile();
        
    }
	// Update is called once per frame
	void Update () {

        UpdateAging();

        if (BasicCharactor.Status == CharactorMovement.CharacterStatus.Idling || BasicCharactor.Status == CharactorMovement.CharacterStatus.Running)
        {
            ApplyMovement();
        }

        CheckCurrentTile();
    }


    public void MoveHorizontal(float value)
    {
        horizontalInput = value;
        
    }
    public void MoveVertical(float value)
    {
        verticalInput = value;
    }

    
    private void ApplyMovement()
    {
        Vector3 vel = horizontalInput * Vector3.right + verticalInput * Vector3.forward;
        float speed = Mathf.Clamp01(vel.magnitude) * maxSpeed;
        if(CurrentTile != null)
        {
            speed = CurrentTile.ModifySpeed(speed,this);
        }
        

        vel.Normalize();
        BasicCharactor.Move(vel, speed);

        //reset the input value
        horizontalInput = 0;
        verticalInput = 0;
    }

    private void CheckCurrentTile()
    {
        Vector3 currentPosition = transform.position;
        BasicTile tile = TilesController.tilesController.GetOnTile(currentPosition);
        if(tile != CurrentTile)
        {
            if(tile != null)
            {
                tile.OnEnterTile(this);
            }
            if(CurrentTile != null)
            {
                CurrentTile.OnExitTile(this);
            }
            CurrentTile = tile;
        }
    }

    private void UpdateAging()
    {
        AgeStage stageOld = CharacterPresets.characterPresets.CheckStageForAge(CurrentAge);
        CurrentAge += Time.deltaTime * LevelGenerationData.levelGenerationData.agingSpeed;

        if(stageOld != CurrentStage && CurrentStage != AgeStage.Death)
        {
            GameController.gameController.ChangeCharacter(currentAge);
            GamePlayUI.gamePlayUI.ChangeIconsColor(CurrentStage);
        }
        
    }



    public void Die()
    {
        MoveHorizontal(0);
        MoveVertical(0);
        BasicCharactor.Move(Vector3.up, 0);
        enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        GameController.gameController.PlayeyDieLogic();
    }

    public void PassLevel()
    {
        MoveHorizontal(0);
        MoveVertical(0);
        BasicCharactor.Move(Vector3.up, 0);
        enabled = false;
        GetComponent<PlayerInput>().enabled = false;
    }


    public void StartJumping()
    {
        BasicCharactor.StartJumping();
        AkSoundEngine.PostEvent("Play_Jump", gameObject);
    }

    public void EndJumping()
    {
        BasicCharactor.EndJumping();
        AkSoundEngine.PostEvent("Play_Land", gameObject);
    }



}
