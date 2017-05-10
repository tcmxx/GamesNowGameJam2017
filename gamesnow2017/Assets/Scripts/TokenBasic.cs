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
        if(!obtained && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Obtained toekn: " + other.gameObject.name);
            OnPlayerObtained(other.gameObject.GetComponent<CharacterControl>());
        }
    }
}
