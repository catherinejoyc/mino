using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmoSounds : MonoBehaviour {


    public AK.Wwise.Event playAtmoSounds;

    // Use this for initialization
    void Start () {

        playAtmoSounds.Post(this.gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
