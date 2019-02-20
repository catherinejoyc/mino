using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour {

    NavMeshAgent m_agent;

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
    public float waitSecBeforeAttack;

    //sprite
    public SpriteRenderer aggroSprite;
    float startTimeSprite = 0;
    public float cooldownSprite;

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
                // Choose the next destination point when the agent gets
                // close to the current one.
                if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f/* && m_idle*/)
                {
                    GoToNextPoint();
                }
                break;
            case State.Alert:
                //Go back to Idle after alertStateDuration
                Debug.Log("ALERT");
                if (alertStartTime + alertStateDuration <= Time.time)
                {
                    UpdateIdleState();
                    Debug.Log("Nothing there...");
                }
                break;
            case State.Hunt:
                // if close to the current one (and no collision detected)
                Debug.Log("Hunting...");
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
        //start alert
        alertStartTime = Time.time;

        //stop movement
        Stop();

        currState = State.Alert;
    }
    void UpdateHuntState(Vector3 pos, float maxDistance)
    {
        //check maxDistance and actual distance to sound
        float currDistance = Vector3.Distance(pos, this.transform.position);
        if (currDistance <= maxDistance)
        {
            //Go into Hunt State
            currState = State.Hunt;

            m_agent.SetDestination(pos); //Track Sound

            //show aggro Sprite ONLY if near player
            //aggroSprite.enabled = true;
            //startTimeSprite = Time.deltaTime;

            Go();
        }
    }
    void Attack()
    {
        Debug.Log("ATTACKING");
        currState = State.Attack;
        Stop();

        //Play Roar

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
}
