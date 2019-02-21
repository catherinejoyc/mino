using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSoundScript : SoundScript {

    PlayerController m_player;
    public float stepIntervall;
    float sneakingStepIntervall;
    float lastStepTime;

    [Header("Volume, that is shown in UI (VolumeIndicator) [0-1]")]
    //Volume (fake Volume, is just for gameplay)
    float volume;
    public float sneakingVolume;

    //Switches
    public AK.Wwise.Switch surfaceDirt;

    private void Awake()
    {
        //[old] Add PlayAudio to Events
        //m_PlayRunningFootstep.AddListener(PlayRunningFootstep);
        //m_PlaySneakingFootstep.AddListener(PlaySneakingFootstep);

        sneakingStepIntervall = stepIntervall * 2;

    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //play Footstep every few sec
            if (Input.GetButton("Sneaking"))
            {
                if (Time.time > lastStepTime + sneakingStepIntervall)
                {
                    m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                    lastStepTime = Time.time;

                    volume = 
                }
            }
            else
            {
                if (Time.time > lastStepTime + stepIntervall)
                {
                    m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                    lastStepTime = Time.time;
                }
            }

            UIManager.MyInstance.VolumeIndicator.value = volume;
        }
        else
            UIManager.MyInstance.VolumeIndicator.value = 0;
    }

    public AudioSource audioSource;
    public AudioClip footstepInBush;
    public AudioClip footstepOnStone;
    public AudioClip footstepOnGravel;
    void PlayRunningFootstep()
    {
        if (audioSource.clip == footstepInBush)// if in bush
        {
            audioSource.volume = 1;
            audioSource.Play();
        }
        else
        {
            audioSource.volume = 0.8f;
            audioSource.Play();
        }
    }
    void PlaySneakingFootstep()
    {
        if (audioSource.clip == footstepInBush)// if in bush
        {
            audioSource.volume = 0.8f;
            audioSource.Play();
        }
        else
        {
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }

    public void ChangeFootstep(int underGround)
    {
        switch(underGround)
        {
            case 1:
                audioSource.clip = footstepOnStone;
                break;
            case 2:
                audioSource.clip = footstepOnGravel;
                break;
            case 3:
                audioSource.clip = footstepInBush;
                break;
            default:
                audioSource.clip = footstepOnStone;
                break;

        }
    }
}
