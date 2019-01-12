using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemySoundScript : MonoBehaviour {

    NavMeshAgent m_agent;
    public float stepIntervall;
    float lastStepTime;

    //sound
    public AudioSource audioSource;
    public AudioClip footstepInBush;
    public AudioClip footstepOnStone;
    public AudioClip footstepOnGravel;

    UnityEvent m_playFootstep = new UnityEvent();

    // Use this for initialization
    void Awake () {
        m_agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update () {
        if (!m_agent.isStopped)
        {
            if (Time.time > lastStepTime + stepIntervall)
            {
                m_playFootstep.Invoke();

                lastStepTime = Time.time;
            }
        }
    }

    void PlayFootStep()
    {

    }
}
