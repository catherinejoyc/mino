using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkScript : MonoBehaviour {

    public PlayerController player;

    //chalk related
    public GameObject sprite_chalk;
    public float maxDistanceToWall;
    float chalk_startTime = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        #region Chalk
        //Use Chalk
        if (/*Input.GetKeyDown(KeyCode.Q) || */Input.GetButtonDown("Fire3"))
        {
            chalk_startTime = Time.time;
        }
        if (/*Input.GetKeyUp(KeyCode.Q) ||*/ Input.GetButtonDown("Fire3"))
        {
            if (player.currentObjectHolding == null)
            {
                if ((Time.time - chalk_startTime) < 0.2f) //draw x if the player pressed the button for a short time
                {
                    DrawX();
                }
                else
                {
                    //delete X
                    DeleteX();
                }
            }
            else
                Debug.Log("Can't draw while holding something!");
        }
        #endregion
    }

    #region Chalk
    private void DrawX()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, player.m_cam.transform.TransformVector(Vector3.forward), out hit, maxDistanceToWall))
        {
            // Add Sprite on wall
            if (!hit.collider.gameObject.CompareTag("x"))
            {
                GameObject x = Instantiate(sprite_chalk, hit.point, Quaternion.LookRotation(-hit.normal)); //hit.collider.gameObject.transform.rotation * Quaternion.Euler(0,180,0)
                x.transform.rotation = hit.transform.rotation;
            }
            else
                Debug.Log("You can't draw here!");
        }
        else
            Debug.Log("No wall detected. Try going closer.");
    }

    private void DeleteX()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, player.m_cam.transform.TransformVector(Vector3.forward), out hit, maxDistanceToWall))
        {
            Debug.Log(hit.collider.name);
            // delete X
            if (hit.collider.gameObject.CompareTag("x"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    #endregion
}
