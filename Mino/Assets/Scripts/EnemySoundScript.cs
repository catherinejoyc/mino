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
    //public AudioClip footstepInBush;
    //public AudioClip footstepOnStone;
    //public AudioClip footstepOnGravel;
    public AK.Wwise.Event footstepEvent;

    public AudioSource aggroSound;

    UnityEvent m_playFootstep = new UnityEvent();

    // Use this for initialization
    void Awake () {
        m_agent = GetComponent<NavMeshAgent>();

        m_playFootstep.AddListener(PlayFootStep);
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
        //if (audioSource.clip == footstepInBush)// if in bush
        //{
        //    audioSource.volume = 1f;
        //    audioSource.Play();
        //}
        //else
        //{
        //    audioSource.volume = 0.8f;
        //    audioSource.Play();
        //}

        footstepEvent.Post(this.gameObject);
    }

    public void ChangeFootstep(int underGround)
    {
        //switch (underGround)
        //{
        //    case 1:
        //        audioSource.clip = footstepOnStone;
        //        break;
        //    case 2:
        //        audioSource.clip = footstepOnGravel;
        //        break;
        //    case 3:
        //        audioSource.clip = footstepInBush;
        //        break;
        //    default:
        //        audioSource.clip = footstepOnStone;
        //        break;
        //}
    }

    public void PlayAggroSound()
    {
        if (!aggroSound.isPlaying)
            aggroSound.Play();
    }
}
