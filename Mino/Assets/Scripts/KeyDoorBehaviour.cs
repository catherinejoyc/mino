using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyDoorBehaviour : MonoBehaviour {

    public float m_speed = 1;
    public float m_moveUp = 4;

    Vector3 m_openStatePos;

    bool m_isOpening = false;

    public bool m_unlocked = false;

    //Checkpoint (nicht mehr nötig)
    //Vector3 m_checkpoint;

    // Use this for initialization
    void Start () {
        m_openStatePos = new Vector3(transform.position.x, m_moveUp, transform.position.z);

        //Checkpoint
        //m_checkpoint = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_isOpening)
            transform.position = Vector3.MoveTowards(transform.position, m_openStatePos, m_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && m_unlocked)
        {
            m_isOpening = true;

            //next Level if not last level
            if (SceneManager.GetActiveScene().buildIndex != 2)
                GameManager.MyInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
            //set new checkpoint (nicht mehr nötig)
            //other.gameObject.GetComponent<PlayerController>().SetCheckpoint(m_checkpoint);
        }
    }

    public void UnlockDoor()
    {
        m_unlocked = true;
    }
}
