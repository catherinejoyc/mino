using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISounds : MonoBehaviour {

    public AK.Wwise.Event playPositiveEvent;
    public AK.Wwise.Event playNegativeEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void PlayPosUISound()
    {
        playPositiveEvent.Post(this.gameObject);
    }
    public void PlayNegUISound()
    {
        playNegativeEvent.Post(this.gameObject);
    }
}
