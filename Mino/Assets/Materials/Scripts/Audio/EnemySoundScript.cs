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

    Underground currUnderground;
    [Header("Ak Switches")]//Switches
    public AK.Wwise.Switch surfaceStone;
    public AK.Wwise.Switch surfaceGravel;
    public AK.Wwise.Switch surfaceGrass;

    [Header("State PlayEvents")]
    public AK.Wwise.Event playAlertState;
    public AK.Wwise.Event playHuntState;
    public AK.Wwise.Event playAttack;

    // Use this for initialization
    void Awake () {
        m_agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update () {
        if (!m_agent.isStopped) //walking
        {
            if (Time.time > lastStepTime + stepIntervall)
            {
                m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                lastStepTime = Time.time;
            }
        }
    }

    public void ChangeFootstep(int underGround)
    {
        //set switches and currUnderground
        switch (underGround)
        {
            case 1: //stone
                surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;
            case 2: //gravel
                surfaceGravel.SetValue(this.gameObject);
                currUnderground = Underground.Gravel;
                break;
            case 3: //grass
                surfaceGrass.SetValue(this.gameObject);
                currUnderground = Underground.Grass;
                break;
            default: //stone
                surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;

        }
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
}
