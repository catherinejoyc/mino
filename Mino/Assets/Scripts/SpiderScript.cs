using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderScript : MonoBehaviour {

    NavMeshAgent m_agent;
    bool m_idle = true;

    //player related
    GameObject m_player = null;
    float m_maxDistanceToPlayer;
    float m_currDistanceToPlayer;
    bool m_IsCloseToPlayer = false;

    //patrol
    public Transform[] points;
    private int m_destPoint = 0;
    public float waitSeconds = 0;

    //stone related
    GameObject tempstone;

    //aggro sound
    public EnemySoundScript sound;

    // Use this for initialization
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.autoBraking = false;

        m_maxDistanceToPlayer = GetComponent<SphereCollider>().radius; //radius of sphere trigger
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player != null && !m_idle) //get distance between player and enemy
        {
            m_currDistanceToPlayer = Vector3.Distance(m_player.transform.position, transform.position);

            if (m_currDistanceToPlayer <= 3 && !m_IsCloseToPlayer) //distance to player closer than 3 m
            {
                //Add listener to loud noises (TrackPlayerThroughWalls) and quiet noises (TrackPlayer)
                m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.AddListener(TrackSoundThroughWalls);
                m_player.GetComponent<PlayerSoundScript>().m_PlaySneakingFootstep.AddListener(TrackSound);

                m_IsCloseToPlayer = true;
            }
            else if (m_currDistanceToPlayer > 3 && m_IsCloseToPlayer) //distance is bigger than 3 m
            {
                m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.RemoveListener(TrackSoundThroughWalls);
                m_player.GetComponent<PlayerSoundScript>().m_PlaySneakingFootstep.RemoveListener(TrackSound);

                m_IsCloseToPlayer = false;
            }
        }

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f/* && m_idle*/)
        {
            if(!m_idle && !backingUp && !fallingInvoked) //if spider is currently chasing (not idling) and not backing up and is not already falling
            {
                fallingInvoked = true;
                m_agent.isStopped = true;
                Invoke("FallDown", waitSeconds);
            }
            else
                GotoNextPoint();
        }

        //attack
        if (attacking)
        {
            t += Time.deltaTime * attackSpeed;
            m_agent.baseOffset = (Mathf.Lerp(7, 1, t));

            if (m_agent.baseOffset == 1)
                BackUp();
        }
        else if (backingUp)
        {
            t += Time.deltaTime * attackSpeed;
            m_agent.baseOffset = Mathf.Lerp(1, 7, t);

            if (m_agent.baseOffset == 7)
            { 
                backingUp = false;
                m_agent.isStopped = false;
            }
        }

            
        //if (m_agent.baseOffset == 1) //spider on ground
        //    BackUp();

        //else if (backingUp) //backing up
        //{
        //    t += Time.deltaTime * attackSpeed;
        //    m_agent.baseOffset = (Mathf.Lerp(7, 1, t));

        //    if (m_agent.baseOffset == 7)
        //        backingUp = false;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") //player enters trigger
        {
            m_player = other.gameObject;
            m_idle = false;

            //Add Listener
            m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.AddListener(TrackSound);
        }
        //Steinchen
        if (other.GetComponent<StoneBehaviour>() != null)
        {
            tempstone = other.gameObject;
            other.GetComponent<StoneBehaviour>().m_playStoneSoundEvent.AddListener(TrackStoneSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player") //player out of hearing range
        {
            //Remove Listener
            m_idle = true;
            m_player.GetComponent<PlayerSoundScript>().m_PlayRunningFootstep.RemoveListener(TrackSound);

            m_player = null;
        }
    }


    #region Track Sound + Attack
    /* Laute Geräusche:
    * Laufen
    * Landen nach Springen
    */

    /* Leise Geräusche:
    * Schleichen
    * ---
    * Steinchen aufprallen (von der Decke und vom Spieler geworfen)
    * ---
    * Kisten/Leitern platzieren
    * Rascheln im Gebüsch/andere Pflanzen)
    */

    void TrackSound() //Track sound if no wall is inbetween
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_player.transform.position - transform.position, out hit, m_maxDistanceToPlayer)) //Check if wall is between player and self
        {
            if (hit.collider.name == "Player")
            {
                // wait x seconds before attack               
                m_agent.isStopped = true;
                m_agent.SetDestination(m_player.transform.position); //Track Player

                //aggro sound
                sound.PlayAggroSound();

                //Invoke("Go", waitSeconds);
                Go();
            }
            else //if ()
            {

            }
        }
    }

    void TrackSoundThroughWalls()
    {
        // wait x seconds before attack
        m_agent.isStopped = true;
        m_agent.SetDestination(m_player.transform.position); //Track Player

        //aggro sound
        sound.PlayAggroSound();

        //Invoke("Go", waitSeconds);
        Go();
    }

    void TrackStoneSound()
    {
        // wait x seconds before attack
        m_agent.isStopped = true;
        m_agent.SetDestination(tempstone.transform.position); //Track Player
        Invoke("Go", waitSeconds);
    }

    void Go()
    {
        m_agent.isStopped = false;
    }

    public float attackSpeed;
    float t;
    bool attacking = false;
    bool backingUp = false;
    bool fallingInvoked = false;

    void FallDown() //Attack
    {
        print("attack");
        if (!attacking)
            t = 0;
        attacking = true; //spider falls down
        fallingInvoked = false;
    }

    void BackUp() //Go back up
    {
        attacking = false;
        backingUp = true;
        t = 0;
    }
    #endregion

    #region Patrol
    void GotoNextPoint()
    {
        // returns if no points have been set up
        if (points.Length == 0)
            return;

        m_agent.isStopped = true;

        // Set the agent to go to the currently selected destination.
        m_agent.destination = points[m_destPoint].position;
        //Invoke("Go", waitSeconds);
        Go();

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        m_destPoint = (m_destPoint + 1) % points.Length;
    }
    #endregion
}
