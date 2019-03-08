using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum Underground
{
    Stone,
    Gravel,
    Grass
}

public class PlayerSoundScript : SoundScript {
    public float stepIntervall;
    float sneakingStepIntervall;
    float lastStepTime;

    //PlayerController
    PlayerController player;

    //Volume (fake Volume, is just for gameplay)
    float volume;
    Underground currUnderground;

    [Header("Volume, that is shown in UI [0-1]")]
    [Header("sneaking volume")]
    public float sVolume;
    [Header("running volumes")]
    public float rVolume_Stone;
    public float rVolume_Gravel;
    public float rVolume_Grass;

    [Header("Ak Switches")]//Switches
    public AK.Wwise.Switch surfaceStone;
    public AK.Wwise.Switch surfaceGravel;
    public AK.Wwise.Switch surfaceGrass;

    public AK.Wwise.Switch walking;
    public AK.Wwise.Switch sneaking;
    public AK.Wwise.Switch land;

    //land
    bool isLanding = false;

    private void Awake()
    {
        sneakingStepIntervall = stepIntervall * 2;
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        //land
        if (!player.m_isGrounded) //in air
        {
            isLanding = true;
        }
        else if (isLanding) //grounded again
        {
            land.SetValue(gameObject);
            isLanding = false;
        }

        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && player.m_isGrounded)
        {
            //play Footstep every few sec
            if (Input.GetButton("Sneaking"))
            {
                //update switch
                sneaking.SetValue(gameObject);

                if (Time.time > lastStepTime + sneakingStepIntervall)
                {
                    //update volume
                    volume = sVolume;

                    m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                    lastStepTime = Time.time;
                }
            }
            else
            {
                //update switch
                walking.SetValue(gameObject);
                if (Time.time > lastStepTime + stepIntervall)
                {
                    //Check underground and update volume
                    switch (currUnderground)
                    {
                        case Underground.Stone:
                            volume = rVolume_Stone;
                            break;
                        case Underground.Gravel:
                            volume = rVolume_Gravel;
                            break;
                        case Underground.Grass:
                            volume = rVolume_Grass;
                            break;
                        default:
                            volume = rVolume_Stone;
                            break;
                    }

                    m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                    lastStepTime = Time.time;
                }
            }
            if (!player.isPushingBox)
                UIManager.MyInstance.VolumeIndicator.value = volume;
        }
        else
        {
            if (!player.isPushingBox)
                UIManager.MyInstance.VolumeIndicator.value = 0;
        }
    }

    //public void ChangeFootstep(int underGround)
    //{
    //    //set switches and currUnderground
    //    switch(underGround)
    //    {
    //        case 1: //stone
    //            surfaceStone.SetValue(this.gameObject);
    //            currUnderground = Underground.Stone;
    //            break;
    //        case 2: //gravel
    //            surfaceGravel.SetValue(this.gameObject);
    //            currUnderground = Underground.Gravel;
    //            break;
    //        case 3: //grass
    //            surfaceGrass.SetValue(this.gameObject);
    //            currUnderground = Underground.Grass;
    //            break;
    //        default: //stone
    //            surfaceStone.SetValue(this.gameObject);
    //            currUnderground = Underground.Stone;
    //            break;

    //    }
    //}
}
