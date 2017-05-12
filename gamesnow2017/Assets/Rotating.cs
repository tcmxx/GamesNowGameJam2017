using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour {

    public float rotationSpeed = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 angles = transform.rotation.eulerAngles;
        angles.y = (angles.y + rotationSpeed*Time.deltaTime) % 360;
        transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z);
    }
}
