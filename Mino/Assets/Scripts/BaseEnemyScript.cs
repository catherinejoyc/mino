using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour {

    NavMeshAgent m_agent;

    //player related
    GameObject m_player = null;
    float m_maxDistanceToPlayer;
    float m_currDistanceToPlayer;
    bool m_IsCloseToPlayer = false;

	// Use this for initialization
	void Start () {
        m_agent = GetComponent<NavMeshAgent>();
        m_maxDistanceToPlayer = GetComponent<SphereCollider>().radius; //radius of sphere trigger
	}
	
	// Update is called once per frame
	void Update () {
        if (m_player != null) //get distance between player and enemy
        {
            m_currDistanceToPlayer = Vector3.Distance(m_player.transform.position, transform.position);

            if (m_currDistanceToPlayer <= 3 && !m_IsCloseToPlayer) //distance to player closer than 3 m
            {
                //Add listener to loud noises (TrackPlayerThroughWalls) and quiet noises (TrackPlayer)
                m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.AddListener(TrackPlayerThroughWalls);
                m_player.GetComponent<PlayerSoundScript>().m_PlaySneakingFootstep.AddListener(TrackPlayer);

                m_IsCloseToPlayer = true;
            }
            else if (m_currDistanceToPlayer > 3 && m_IsCloseToPlayer) //distance is greater than 3 m
            {
                m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.RemoveListener(TrackPlayerThroughWalls);
                m_player.GetComponent<PlayerSoundScript>().m_PlaySneakingFootstep.RemoveListener(TrackPlayer);

                m_IsCloseToPlayer = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") //player enters trigger
        {
            m_player = other.gameObject;

            //Add Listener
            m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.AddListener(TrackPlayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player") //player out of hearing range
        {
            //Remove Listener
            m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.RemoveListener(TrackPlayer);

            m_player = null;
        }
    }

    void TrackPlayer() //Track player if no wall is inbetween
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_player.transform.position - transform.position, out hit, m_maxDistanceToPlayer)) //Check if wall is between player and self
        {
            if (hit.collider.name == "Player")
            {
                m_agent.SetDestination(m_player.transform.position); //Track Player
            }
        }
    }

    void TrackPlayerThroughWalls()
    {
        m_agent.SetDestination(m_player.transform.position);
    }
}
