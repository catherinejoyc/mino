using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyDoorBehaviour : SoundScript {

    public float m_speed;
    public float m_moveUp;

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
	
	void Update () {
        if (m_isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_openStatePos, m_speed * Time.deltaTime);

            //post Sound Event
            m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && m_unlocked)
        {
            m_isOpening = true;

            //fade to black
            UIManager.MyInstance._fadeScreen.gameObject.SetActive(true);
            UIManager.MyInstance.FadeToBlack();

            //after blend load next level
            Invoke("LoadNextLevel", 1);

            //set new checkpoint (nicht mehr nötig)
            //other.gameObject.GetComponent<PlayerController>().SetCheckpoint(m_checkpoint);
        }
    }

    void LoadNextLevel()
    {
        //next Level
        GameManager.MyInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void UnlockDoor()
    {
        m_unlocked = true;
    }
}
