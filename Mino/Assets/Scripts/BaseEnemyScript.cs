using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle,
    Alert,
    Hunt,
    Attack
}

public class BaseEnemyScript : MonoBehaviour {

    NavMeshAgent m_agent;

    public State currState = State.Idle;

    //Idle
    public Transform[] points;
    private int m_destPoint = 0;
    public float waitSeconds = 0;

    //Alert
    float alertStartTime;
    [Tooltip("enemy leaves alert state after x seconds")]
    public float alertStateDuration;
    bool alertSoundPlaying = false;

    //Hunt
    bool huntSoundPlaying = false;
    Vector3 _huntDestination;
    float _hearingDistance;
    [Tooltip("enemy leaves hunt state after x seconds")]
    public float huntStateDuration;
    float huntStartTime;

    //Attack
    public float attackRange;
    public float waitSecBeforeAttack;
    bool attackSoundPlaying = false;

    //sprite
    public SpriteRenderer aggroSprite;
    float startTimeSprite = 0;
    public float cooldownSprite;
    bool _showingSprite;

	// Use this for initialization
	void Start () {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.autoBraking = false;

	}
	
	// Update is called once per frame
	void Update () {

        //Behaviour
        switch (currState)
        {
            case State.Idle:
                _showingSprite = false;
                // Choose the next destination point when the agent gets
                // close to the current one.
                if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f/* && m_idle*/)
                {
                    GoToNextPoint();
                }
                break;
            case State.Alert:
                UpdateAlertState();
                break;
            case State.Hunt:
                UpdateHuntState(_huntDestination, _hearingDistance);
                break;
            case State.Attack:
                Attack();
                break;
        }

        FlickerSprite();
    }

    private void OnTriggerEnter(Collider other)
    {
        // --- New Sound Tracking System
        if (other.GetComponent<SoundScript>() != null)
        {
            //Add UpdateAggroState to SoundEvent of target
            other.GetComponent<SoundScript>().m_SoundEvent.AddListener(UpdateAggroState);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // --- New SoundSystem
        if (other.GetComponent<SoundScript>() != null)
        {
            //Add UpdateAggroState to SoundEvent of target
            other.GetComponent<SoundScript>().m_SoundEvent.RemoveListener(UpdateAggroState);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if colliding with something hittable eg. Player or Boxes
        if (collision.collider.GetComponent<IHittable>() != null)
        {
            //turn to collision
            Vector3 targetDir = collision.transform.position - this.transform.position;
            Vector3 newDir = Vector3.RotateTowards(this.transform.forward, targetDir, Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            //Attack
            Attack();
        }
    }

    //Update States
    void UpdateAggroState(Vector3 pos, float maxDistance)
    {
        if (currState == State.Idle)
        {
            UpdateAlertState();
        }
        else if (currState == State.Alert)
        {
            UpdateHuntState(pos, maxDistance);
        }
    }
    void UpdateAlertState()
    {
        //switch state
        if (currState != State.Alert)
        {
            alertStartTime = Time.time;
            currState = State.Alert;
        }
        _showingSprite = false;

        //Go back to Idle after alertStateDuration
        if (alertStartTime + alertStateDuration <= Time.time)
        {
            UpdateIdleState();
        }

        //play sound once
        if (!alertSoundPlaying) //play once
        {
            GetComponent<EnemySoundScript>().PlayAlertStateSound();
            //enable sounds for next play
            huntSoundPlaying = false; 
            attackSoundPlaying = false;

            alertSoundPlaying = true;
        }

        //stop movement
        Stop();
    }
    void UpdateHuntState(Vector3 pos, float maxDistance)
    {
        //check maxDistance and actual distance to sound
        float currDistance = Vector3.Distance(pos, this.transform.position);
        if (currDistance <= maxDistance)
        {
            //save parameteres in instancevariables
            _huntDestination = pos;
            _hearingDistance = maxDistance;

            //switch state
            if  (currState != State.Hunt)
            {
                huntStartTime = Time.time;
                currState = State.Hunt;
            }
            //duration check, Go back to Idle after huntStateDuration
            if (huntStartTime + huntStateDuration <= Time.time)
            {
                UpdateIdleState();
            }

            //play sound once
            if (!huntSoundPlaying) //play once
            {
                GetComponent<EnemySoundScript>().PlayHuntStateSound();

                //enable sounds for next play
                alertSoundPlaying = false;
                attackSoundPlaying = false;

                huntSoundPlaying = true;
            }

            m_agent.SetDestination(pos); //Track Sound

            _showingSprite = true;

            Go();
        }

        // if close to the current one (and no collision detected)                
        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
        {
            Attack();
        }
    }
    void Attack()
    {
        currState = State.Attack;
        Stop();

        _showingSprite = true;

        //Play Roar
        if (!attackSoundPlaying) //play once
        {
            GetComponent<EnemySoundScript>().PlayAttackSound();

            //enable sounds for next play
            huntSoundPlaying = false;
            alertSoundPlaying = false;

            attackSoundPlaying = true;
        }

        //Raycast in front of Enemy
        Invoke("Hit", waitSecBeforeAttack);

        UpdateIdleState();
    }
    void Hit()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackRange, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
        {
            Debug.Log("Did Hit");

            //React to hit
            if (hit.collider.GetComponent<IHittable>() != null)
            {
                hit.collider.GetComponent<IHittable>().ReactToHit();
            }
        }
    }
    void UpdateIdleState()
    {
        //continue patrol
        Go();
        GoToNextPoint();
        currState = State.Idle;
    }

    #region Patrol
    void GoToNextPoint()
    {
        // returns if no points have been set up
        if (points.Length == 0)
            return;

        m_agent.isStopped = true;

        // Set the agent to go to the currently selected destination.
        m_agent.destination = points[m_destPoint].position;
        Invoke("Go", waitSeconds);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        m_destPoint = (m_destPoint + 1) % points.Length;
    }
    void Go()
    {
        m_agent.isStopped = false;
    }
    void Stop()
    {
        m_agent.isStopped = true;
    }
    #endregion

    #region FlickerSprite
    void FlickerSprite()
    {
        if (_showingSprite)
        {
            aggroSprite.transform.LookAt(FindObjectOfType<PlayerController>().transform.position, Vector3.up);
            aggroSprite.enabled = false;
            Invoke("ShowSprite", cooldownSprite);
            aggroSprite.enabled = false;
        }
        else if (startTimeSprite < Time.time + cooldownSprite)
            aggroSprite.enabled = false;

    }
    void ShowSprite()
    {
        print("show sprite @" + this.gameObject.name);
        aggroSprite.enabled = true;
    }
    #endregion
}
