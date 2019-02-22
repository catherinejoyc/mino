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

    private void Awake()
    {
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
                    //update volume
                    volume = sVolume;

                    m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                    lastStepTime = Time.time;
                }
            }
            else
            {
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

            UIManager.MyInstance.VolumeIndicator.value = volume;
        }
        else
            UIManager.MyInstance.VolumeIndicator.value = 0;
    }

    public void ChangeFootstep(int underGround)
    {
        //set switches and currUnderground
        switch(underGround)
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
}
