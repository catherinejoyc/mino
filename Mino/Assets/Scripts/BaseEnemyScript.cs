using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour {

    NavMeshAgent m_agent;
    bool m_idle = true;

    //player related
    GameObject m_player = null;
    float m_maxDistanceToPlayer;
    float m_currDistanceToPlayer;
    bool m_IsCloseToPlayer = false;

    //States
    enum State
    {
        Idle,
        Alert,
        Hunt,
        Attack
    }
    State currState = State.Idle;

    //Idle
    public Transform[] points;
    private int m_destPoint = 0;
    public float waitSeconds = 0;

    //Alert
    float alertStartTime;
    public float alertStateDuration;

    //Attack
    public float attackRange;

    //stone related
    GameObject tempstone;

    //aggro sound
    public EnemySoundScript sound;

    //sprite
    public SpriteRenderer aggroSprite;
    float startTimeSprite = 0;
    public float cooldownSprite;

	// Use this for initialization
	void Start () {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.autoBraking = false;

        m_maxDistanceToPlayer = GetComponent<SphereCollider>().radius; //radius of sphere trigger
	}
	
	// Update is called once per frame
	void Update () {
        #region [old] change from soundTracking Player sneaking and running
        //if (m_player != null && !m_idle) //get distance between player and enemy
        //{
        //    m_currDistanceToPlayer = Vector3.Distance(m_player.transform.position, transform.position);

        //    if (m_currDistanceToPlayer <= 1 && !m_IsCloseToPlayer) //distance to player closer than 1 m
        //    {
        //        //Add listener to loud noises(TrackPlayerThroughWalls) and quiet noises(TrackPlayer)
        //        m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.AddListener(TrackSoundThroughWalls);
        //        m_player.GetComponent<PlayerSoundScript>().m_PlaySneakingFootstep.AddListener(TrackSound);

        //        m_IsCloseToPlayer = true;
        //    }
        //    else if (m_currDistanceToPlayer > 1 && m_IsCloseToPlayer) //distance is bigger than 1 m
        //    {
        //        m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.RemoveListener(TrackSoundThroughWalls);
        //        m_player.GetComponent<PlayerSoundScript>().m_PlaySneakingFootstep.RemoveListener(TrackSound);

        //        m_IsCloseToPlayer = false;
        //    }
        //}
        #endregion

        //Behaviour
        switch (currState)
        {
            case State.Idle:
                // Choose the next destination point when the agent gets
                // close to the current one.
                if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f/* && m_idle*/)
                {
                    GoToNextPoint();
                }
                break;
            case State.Alert:
                //Go back to Idle after alertStateDuration
                if (alertStartTime + alertStateDuration <= Time.time)
                {
                    UpdateIdleState();
                }
                break;
            case State.Hunt:
                // if close to the current one (and no collision detected)
                if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f/* && m_idle*/)
                {
                    Attack();
                }
                break;
            case State.Attack:
                break;
        }

        //Aggro Sprite
        if (startTimeSprite <= Time.deltaTime + cooldownSprite)
        {
            aggroSprite.gameObject.transform.LookAt(FindObjectOfType<PlayerController>().transform);
            aggroSprite.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        #region old
        //if (other.name == "Player") //player enters trigger
        //{
        //    m_player = other.gameObject;
        //    m_idle = false;

        //    //Add Listener
        //    m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.AddListener(TrackSound);
        //}
        ////Steinchen
        //if (other.GetComponent<StoneBehaviour>() != null)
        //{
        //    tempstone = other.gameObject;
        //    other.GetComponent<StoneBehaviour>().m_playStoneSoundEvent.AddListener(TrackStoneSound);
        //}
        #endregion

        // --- New Sound Tracking System
        if (other.GetComponent<SoundScript>() != null)
        {
            //Add UpdateAggroState to SoundEvent of target
            other.GetComponent<SoundScript>().m_SoundEvent.AddListener(UpdateAggroState);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        #region old
        //if (other.name == "Player") //player out of hearing range
        //{
        //    //Remove Listener
        //    m_idle = true;
        //    m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.RemoveListener(TrackSound);

        //    m_player = null;
        //}
        #endregion

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
            //Attack
            Attack();
        }
    }

    //Update States
    void UpdateAggroState(Vector3 pos)
    {
        if (currState == State.Idle)
        {
            UpdateAlertState();
        }
        else if (currState == State.Alert)
        {
            UpdateHuntState(pos);
        }
    }
    void UpdateAlertState()
    {
        //start alert
        alertStartTime = Time.time;

        //stop movement
        Stop();

        currState = State.Alert;
    }
    void UpdateHuntState(Vector3 pos)
    {
        //check maxDistance and actual distance to sound
        float currDistance = Vector3.Distance(pos, this.transform.position);
        if (currDistance <= m_maxDistanceToPlayer)
        {
            //Go into Hunt State
            currState = State.Hunt;

            m_agent.SetDestination(pos); //Track Sound

            #region old
            ////aggro sound
            //sound.PlayAggroSound();
            #endregion

            //show aggro Sprite ONLY if near player
            //aggroSprite.enabled = true;
            //startTimeSprite = Time.deltaTime;

            Go();
        }
    }
    void Attack()
    {
        currState = State.Attack;
        Stop();

        //Play Roar

        //Raycast in front of Enemy
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            
            //React to hit
            if(hit.collider.GetComponent<IHittable>() != null)
            {
                hit.collider.GetComponent<IHittable>().ReactToHit();
            }

            UpdateIdleState();
        }
    }
    void UpdateIdleState()
    {
        //continue patrol
        Go();
        currState = State.Idle;
    }


    #region Track Sound (old)
    //void TrackSound() //Track sound if no wall is inbetween
    //{
    //    RaycastHit hit;
    //    if (m_player != null)
    //    {
    //        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_player.transform.position - transform.position, out hit, m_maxDistanceToPlayer)) //Check if wall is between player and self
    //        {
    //            if (hit.collider.name == "Player")
    //            {
    //                // wait x seconds before attack               
    //                m_agent.isStopped = true;
    //                m_agent.SetDestination(m_player.transform.position); //Track Player

    //                //aggro sound
    //                sound.PlayAggroSound();
    //                //show aggro Sprite
    //                aggroSprite.enabled = true;
    //                startTimeSprite = Time.deltaTime;

    //                Invoke("Go", waitSeconds);

    //            }
    //        }
    //    }
    //}

    //void TrackSoundThroughWalls()
    //{
    //    // wait x seconds before attack
    //    m_agent.isStopped = true;
    //    m_agent.SetDestination(m_player.transform.position); //Track Player

    //    //aggro sound
    //    sound.PlayAggroSound();
    //    //show aggro Sprite
    //    aggroSprite.enabled = true;
    //    startTimeSprite = Time.deltaTime;

    //    Invoke("Go", waitSeconds);
    //}

    //void TrackStoneSound()
    //{
    //    // wait x seconds before go
    //    m_agent.isStopped = true;
    //    m_agent.SetDestination(tempstone.transform.position);
    //    Invoke("Go", waitSeconds);
    //}
    #endregion

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
}
