using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureDoor : MonoBehaviour {

    public float m_speed = 1;
    public float m_moveUp = 4;

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
            transform.position = Vector3.MoveTowards(transform.position, m_openStatePos, m_speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_closedStatePos, m_speed * Time.deltaTime);
    }

    public void UnlockDoor()
    {
        m_isOpening = true;
    }

    public void CloseDoor()
    {
        m_isOpening = false;
    }
}
