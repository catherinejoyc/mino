using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeadphoneSceneScript : MonoBehaviour {

    bool nextFrame = false;
    public Image screen;
    public Sprite spr_zeitdruck;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Press Any Key to continue#
        if (Input.anyKeyDown)
        {
            if (nextFrame) //Start
            {
                int lvl = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(lvl + 1);
            }
            else //show Zeitdruck
            {
                screen.sprite = spr_zeitdruck;
                nextFrame = true;
            }
        }
	}
}
