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

    [Header("Ak Switches")]//Switches
    public AK.Wwise.Switch surfaceStone;
    public AK.Wwise.Switch surfaceGravel;
    public AK.Wwise.Switch surfaceGrass;
    public AK.Wwise.Switch surfaceBush;

    [Header("State PlayEvents")]
    public AK.Wwise.Event playAlertState;
    public AK.Wwise.Event playHuntState;
    public AK.Wwise.Event playAttack;

    //Occlusion RTPC
    public AK.Wwise.RTPC occlusionRTPC;

    // Use this for initialization
    void Awake () {
        m_agent = GetComponent<NavMeshAgent>();
        surfaceStone.SetValue(this.gameObject);

        maxDistance = AkSoundEngine.GetMaxRadius(this.gameObject);
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
        //set switches
        switch (underGround)
        {
            case 1: //stone
                surfaceStone.SetValue(this.gameObject);
                break;
            case 2: //gravel
                surfaceGravel.SetValue(this.gameObject);
                break;
            case 3: //grass
                surfaceGrass.SetValue(this.gameObject);
                break;
            case 4: //bush
                surfaceBush.SetValue(this.gameObject);
                break;
            default: //stone
                surfaceStone.SetValue(this.gameObject);
                break;
        }
        Debug.Log(underGround);
    }

    //Play in BaseEnemyScript
    public void PlayAlertStateSound()
    {
        Debug.Log("play alert state sound");
        playAlertState.Post(this.gameObject);
    }

    public void PlayHuntStateSound()
    {
        Debug.Log("play hunt state sound");
        playHuntState.Post(this.gameObject);
    }

    public void PlayAttackSound()
    {
        Debug.Log("play attack sound");
        playAttack.Post(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
