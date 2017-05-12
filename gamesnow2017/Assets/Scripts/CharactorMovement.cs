using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CharactorMovement : MonoBehaviour {


    public float acceleration = 1.0f;
    public float deceleration = 1.0f;

    public float jumpingSpeed = 1.0f;

    public Transform modelTransform;
    public Animator animator;

    
    private Vector3 desiredVelocity;

    private CharacterController characterController;
    public Vector3 CurrentVelocity { get; set; }


    public enum CharacterStatus
    {
        Idling, Running, Jumping
    }
    public CharacterStatus Status { get; private set; }



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if(Status == CharacterStatus.Idling || Status == CharacterStatus.Running)
        {
            UpdateRunning();
        }else if(Status == CharacterStatus.Jumping)
        {
            UpdateJumping();
        }

    }

    private void UpdateRunning()
    {
        //update the velocity
        Vector3 desiredSpeedTangent = desiredVelocity.normalized;
        if (desiredVelocity.magnitude <= 0.01f)
        {
            desiredSpeedTangent = CurrentVelocity.normalized;
            desiredVelocity = Vector3.zero;
        }
        float speedTangent = Vector3.Dot(CurrentVelocity, desiredSpeedTangent);
        float speedTangentDiff = speedTangent - desiredVelocity.magnitude;
        Vector3 vNormal = CurrentVelocity - speedTangent * desiredSpeedTangent;

        //deceleration part


        if (speedTangent < 0 || speedTangentDiff > 0 || desiredVelocity.magnitude <= 0.01f)
        {
            //deceleration all direction
            float magnitude = CurrentVelocity.magnitude - deceleration * Time.fixedDeltaTime;
            if (magnitude <= 0)
            {
                magnitude = 0;
            }
            else if (speedTangentDiff > 0 && magnitude < desiredVelocity.magnitude)
            {
                magnitude = desiredVelocity.magnitude;
            }

            CurrentVelocity = CurrentVelocity.normalized * magnitude;
        }
        else
        {
            //decelerate only the normal v
            float magnitude = vNormal.magnitude - deceleration * Time.fixedDeltaTime;
            if (magnitude <= 0)
            {
                magnitude = 0;
            }
            CurrentVelocity = speedTangent * desiredVelocity.normalized + vNormal.normalized * magnitude;
        }

        //acceleration part
        //get new velocity after deceleration
        desiredSpeedTangent = desiredVelocity.normalized;
        if (desiredVelocity.magnitude <= 0.01f)
        {
            desiredSpeedTangent = CurrentVelocity.normalized;
            desiredVelocity = Vector3.zero;
        }
        speedTangent = Vector3.Dot(CurrentVelocity, desiredSpeedTangent);
        speedTangentDiff = speedTangent - desiredVelocity.magnitude;
        vNormal = CurrentVelocity - speedTangent * desiredSpeedTangent;


        bool speedDiffPos = speedTangentDiff > 0;
        speedTangentDiff += Time.fixedDeltaTime * acceleration * (speedDiffPos ? -1 : 1);
        if(speedDiffPos && speedTangentDiff <= 0)
        {
            speedTangentDiff = 0;
        }else if (!speedDiffPos && speedTangentDiff >= 0)
        {
            speedTangentDiff = 0;
        }

        CurrentVelocity = vNormal + desiredVelocity + speedTangentDiff * desiredSpeedTangent;

        //assign the rigidbody velocity
        if(CurrentVelocity.magnitude <= 0.01f)
        {
            CurrentVelocity = Vector3.zero;
        }
        characterController.SimpleMove(CurrentVelocity);
        //Debug.Log(CurrentVelocity);
        //update the direction
        Vector3 lookat = CurrentVelocity;
        lookat.y = 0;
        if (lookat.magnitude > 0.01f)
        {
            modelTransform.localRotation = Quaternion.LookRotation(lookat.normalized, Vector3.up);
            animator.SetBool("Running", true);
            Status = CharacterStatus.Running;
        }
        else
        {
            animator.SetBool("Running", false);
            Status = CharacterStatus.Idling;
        }
    }

    private void UpdateJumping()
    {
        transform.position += Time.fixedDeltaTime * CurrentVelocity.normalized* jumpingSpeed;
        
    }

    public void Move(Vector3 direction, float desiredSpeed)
    {
        desiredVelocity = direction.normalized * desiredSpeed;
    }


    public void StartJumping()
    {
        Status = CharacterStatus.Jumping;
        animator.SetBool("Jumping", true);
    }

    public void EndJumping()
    {
        Status = CharacterStatus.Idling;
        animator.SetBool("Jumping", false);
    }



}
