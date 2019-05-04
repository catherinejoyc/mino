using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureDoor : SoundScript {

    //Stop Events
    public AK.Wwise.Event stopEvent;
    //Switch (Open and Closing)
    public AK.Wwise.Switch stateOpening;
    public AK.Wwise.Switch stateClosing;


    public float m_speed;
    public float m_moveUp;

    Vector3 m_openStatePos;
    Vector3 m_closedStatePos;

    bool m_isOpening = false;

    //Checkpoint (nicht mehr nötig)
    //Vector3 m_checkpoint;

    // Use this for initialization
    void Start()
    {
        m_closedStatePos = transform.position;
        m_openStatePos = new Vector3(transform.position.x, m_moveUp, transform.position.z);

        //Checkpoint
        //m_checkpoint = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_openStatePos, m_speed * Time.deltaTime);

            //if (transform.position.y == m_openStatePos.y) //if already open
            //{
            //    stopEvent.Post(this.gameObject);
            //    Debug.Log("Stop Event; " + transform.position + " --- " + m_openStatePos);
            //}
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_closedStatePos, m_speed * Time.deltaTime);

            //if (transform.position.y == m_closedStatePos.y) //if already closed
            //{
            //    stopEvent.Post(this.gameObject);
            //    Debug.Log("Stop Event; " + transform.position + " --- " + m_openStatePos);
            //}
        }
    }

    public void UnlockDoor()
    {
        m_isOpening = true;
        //Set Switch on opening
        stateOpening.SetValue(this.gameObject);
        //post Sound Event
        m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
        Debug.Log("Play Event");
    }

    public void CloseDoor()
    {
        m_isOpening = false;
        //Set Switch on closing
        stateClosing.SetValue(this.gameObject);
        //post Sound Event
        m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
        Debug.Log("Play Event");
    }
}
