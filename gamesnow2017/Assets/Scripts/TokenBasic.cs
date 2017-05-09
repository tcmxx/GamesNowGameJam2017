using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenBasic : MonoBehaviour {


    protected bool obtained = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnPlayerObtained(CharacterControl character)
    {
        obtained = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(!obtained && other.attachedRigidbody != null && other.attachedRigidbody.gameObject.CompareTag("Player"))
        {
            Debug.Log("Obtained toekn: " + other.attachedRigidbody.gameObject.name);
            OnPlayerObtained(other.attachedRigidbody.gameObject.GetComponent<CharacterControl>());
        }
    }
}
