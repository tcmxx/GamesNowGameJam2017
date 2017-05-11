﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBasic : MonoBehaviour {

    protected bool obtained = false;
    protected CharacterControl effectedCharacter = null;

    private float buffLeftTime = 0;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public virtual void Update () {
        if(effectedCharacter != null && obtained)
            BuffUpdate();

        if(buffLeftTime > 0)
        {
            buffLeftTime -= Time.deltaTime;
            if(buffLeftTime <= 0)
            {
                RemoveBuff();
            }
        }
    }

    public virtual void OnPlayerObtained(CharacterControl character)
    {
        obtained = true;
    }

    public virtual void BuffUpdate()
    {

    }

    public virtual void OnBuffApplied()
    {

    }

    public virtual void OnBuffRemoved()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(!obtained && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Obtained toekn: " + other.gameObject.name);
            effectedCharacter = other.gameObject.GetComponent<CharacterControl>();
            OnPlayerObtained(effectedCharacter);
        }
    }


    public void ApplyAsBuff(float lastTime, CharacterControl character)
    {
        effectedCharacter = character;
        buffLeftTime = lastTime;
        transform.SetParent(character.transform);
        OnBuffApplied();
    }

    public void RemoveBuff()
    {
        OnBuffRemoved();
        Destroy(gameObject);
    } 

}
