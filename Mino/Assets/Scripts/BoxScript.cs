using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour, IHittable {

    bool damaged = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReactToHit()
    {
        //if damaged, destroy itself
        if (damaged)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Change Normal Map + damaged
            damaged = true;
        }
    }
}
