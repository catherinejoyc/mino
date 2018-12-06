using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour {

    NavMeshAgent m_agent;
    GameObject m_player;
    float m_maxDistanceToPlayer;

	// Use this for initialization
	void Start () {
        m_agent = GetComponent<NavMeshAgent>();
        m_maxDistanceToPlayer = GetComponent<SphereCollider>().radius;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TrackPlayer() //Track player if no wall is inbetween
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_player.transform.position - transform.position, out hit, m_maxDistanceToPlayer))
        {
            if (hit.collider.name == "Player")
            {
                //m_agent.SetDestination(m_player);
            }
        }
    }
}
