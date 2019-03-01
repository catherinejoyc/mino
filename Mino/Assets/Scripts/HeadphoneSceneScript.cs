using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadphoneSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Press Any Key to continue#
        if (Input.anyKey)
        {
            int lvl = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(lvl+1);
        }
	}
}
