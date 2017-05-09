using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharactorMovement))]
public class CharacterControl : MonoBehaviour {


    public static CharacterControl mainCharacter;

    public enum AgeStage
    {
        Kid,
        Teen,
        Adult,
        Old,
        Death
    }
    [System.Serializable]
    public struct AgeStageSetting
    {
        public AgeStage stage;
        public float maxAge;
    }
    public float currentAge;
    public AgeStage currentStage;
    public AgeStageSetting[] ageStageSetting;

    public float ageChangingSpeed = 0.5f;



    public float maxSpeed;



    private float horizontalInput;
    private float verticalInput;

    public CharactorMovement BasicCharactor { get; set; }
    public BasicTile CurrentTile { get; set; }


    private void Awake()
    {
        BasicCharactor = GetComponent<CharactorMovement>();
        mainCharacter = this;
    }
    // Use this for initialization
    void Start()
    {
    }
	// Update is called once per frame
	void Update () {

        UpdateAging();
        ApplyMovement();
        CheckCurrentTile();
    }

    public void InitializeAge(float age)
    {
        currentAge = age;
        currentStage = CheckStageForAge(currentAge);
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
        currentAge += Time.deltaTime * ageChangingSpeed;
        AgeStage stage = CheckStageForAge(currentAge);
        if(stage == AgeStage.Death && currentStage != AgeStage.Death)
        {
            Die();
        }
        currentStage = stage;
    }

    public AgeStage CheckStageForAge(float age)
    {
        foreach(var s in ageStageSetting)
        {
            if(age < s.maxAge)
            {
                return s.stage;
            }
        }

        return AgeStage.Kid;
    }

    public void Die()
    {

    }

    public void PassLevel()
    {

    }


}
