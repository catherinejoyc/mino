using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemySoundScript : SoundScript {

    public BaseEnemyScript m_enemy;
    NavMeshAgent m_agent;
    public float stepIntervall;
    float lastStepTime;
    float maxDistance;

    enum Underground
    {
        Stone,
        Gravel,
        Grass,
        Bush
    }
    Underground currUnderground;

    //[Header("Ak Switches")]//Switches
    //public AK.Wwise.Switch surfaceStone;
    //public AK.Wwise.Switch surfaceGravel;
    //public AK.Wwise.Switch surfaceGrass;
    //public AK.Wwise.Switch surfaceBush;

    [Header("Footstep PlayEvents")]//footstep playevents
    public AK.Wwise.Event footstepGravel;
    public AK.Wwise.Event footstepGrass;
    public AK.Wwise.Event footstepBush;

    [Header("State PlayEvents")]
    public AK.Wwise.Event playAlertState;
    public AK.Wwise.Event playHuntState;
    public AK.Wwise.Event playAttack;

    //Occlusion RTPC
    public AK.Wwise.RTPC occlusionRTPC;

    // Use this for initialization
    void Awake () {
        m_agent = GetComponent<NavMeshAgent>();
        //surfaceStone.SetValue(this.gameObject);

        maxDistance = AkSoundEngine.GetMaxRadius(this.gameObject);

        currUnderground = Underground.Stone;
    }

	// Update is called once per frame
	void Update () {

        if (!m_agent.isStopped) //walking
        {
            if (Time.time > lastStepTime + stepIntervall)
            {
                switch (currUnderground)
                {
                    case Underground.Stone:
                        m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
                        break;
                    case Underground.Gravel:
                        footstepGravel.Post(this.gameObject);
                        break;
                    case Underground.Grass:
                        footstepGrass.Post(this.gameObject);
                        break;
                    case Underground.Bush:
                        footstepBush.Post(this.gameObject);
                        break;
                    default:
                        m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
                        break;
                }
                lastStepTime = Time.time;
            }
        }
    }

    public void ChangeFootstep(int underGround)
    {
        //set switches
        switch (underGround)
        {
            case 1: //stone
                //surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;
            case 2: //gravel
                //surfaceGravel.SetValue(this.gameObject);
                currUnderground = Underground.Gravel;
                break;
            case 3: //grass
                //surfaceGrass.SetValue(this.gameObject);
                currUnderground = Underground.Grass;
                break;
            case 4: //bush
                //surfaceBush.SetValue(this.gameObject);
                currUnderground = Underground.Bush;
                break;
            default: //stone
                //surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;
        }
        Debug.Log(underGround);
    }

    //Play in BaseEnemyScript
    public void PlayAlertStateSound()
    {
        playAlertState.Post(this.gameObject);
    }

    public void PlayHuntStateSound()
    {
        playHuntState.Post(this.gameObject);
    }

    public void PlayAttackSound()
    {
        playAttack.Post(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
