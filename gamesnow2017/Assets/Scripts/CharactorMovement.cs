using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorMovement : MonoBehaviour {


    public float acceleration = 1.0f;
    public float deceleration = 1.0f;

    public Transform modelTransform;
    public Animator animator;

    
    private Vector3 desiredVelocity;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        //update the velocity
        Vector3 desiredSpeedTangent = desiredVelocity.normalized;
        if (desiredVelocity.magnitude <= 0.01f)
        {
            desiredSpeedTangent = currentVelocity.normalized;
        }
        float speedTangent = Vector3.Dot(currentVelocity, desiredSpeedTangent);
        float speedTangentDiff = speedTangent - desiredVelocity.magnitude;
        Vector3 vNormal = currentVelocity - speedTangent * desiredSpeedTangent;

        //deceleration part


        if(speedTangent < 0 || speedTangentDiff > 0 || desiredVelocity.magnitude <= 0.01f)
        {
            //deceleration all direction
            float magnitude = currentVelocity.magnitude - deceleration * Time.fixedDeltaTime;
            if(magnitude <= 0)
            {
                magnitude = 0;
            }else if(speedTangentDiff > 0 && magnitude < desiredVelocity.magnitude)
            {
                magnitude = desiredVelocity.magnitude;
            }

            currentVelocity = currentVelocity.normalized * magnitude;
        }
        else
        {
            //decelerate only the normal v
            float magnitude = vNormal.magnitude - deceleration * Time.fixedDeltaTime;
            if (magnitude <= 0)
            {
                magnitude = 0;
            }
            currentVelocity = speedTangent* desiredVelocity.normalized + vNormal.normalized * magnitude;
        }

        //acceleration part
        //get new velocity after deceleration
        desiredSpeedTangent = desiredVelocity.normalized;
        if(desiredVelocity.magnitude <=0.01f)
        {
            desiredSpeedTangent = currentVelocity.normalized;
        }
        speedTangent = Vector3.Dot(currentVelocity, desiredSpeedTangent);
        speedTangentDiff = speedTangent - desiredVelocity.magnitude;
        vNormal = currentVelocity - speedTangent * desiredSpeedTangent;
        
        speedTangentDiff += Time.fixedDeltaTime * acceleration * ( speedTangentDiff > 0?-1:1);
        currentVelocity = vNormal + desiredVelocity + speedTangentDiff * desiredSpeedTangent;

        //assign the rigidbody velocity
        rb.velocity = currentVelocity;

        //update the direction

        Vector3 lookat = currentVelocity;
        lookat.y = 0;
        if (lookat.magnitude > 0.01f)
        {
            modelTransform.localRotation = Quaternion.LookRotation(lookat.normalized, Vector3.up);
            animator.SetBool("Running", true);
        }else
        {
            animator.SetBool("Running", false);
        }
    }


    public void Move(Vector3 direction, float desiredSpeed)
    {
        desiredVelocity = direction.normalized * desiredSpeed;
    }

}
