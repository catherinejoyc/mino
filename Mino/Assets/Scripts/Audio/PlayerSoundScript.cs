using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum Underground
{
    Stone,
    Gravel,
    Grass,
    Bush
}

public class PlayerSoundScript : SoundScript {
    public float stepIntervall;
    public LayerMask rayCastLayer;

    float sneakingStepIntervall;
    float lastStepTime;

    //PlayerController
    PlayerController player;
    //Camera
    Camera m_cam;

    //Volume (fake Volume, is just for gameplay)
    float volume;
    Underground currUnderground;

    [Header("Volume, that is shown in UI [0-1]")]
    [Header("sneaking volume")]
    public float sVolume;
    public float sInBushVolume;
    [Header("running volumes")]
    public float rVolume_Stone;
    public float rVolume_Gravel;
    public float rVolume_Grass;
    public float rVolume_Bush;

    [Header("Ak Switches")]//Switches
    public AK.Wwise.Switch surfaceStone;
    public AK.Wwise.Switch surfaceGravel;
    public AK.Wwise.Switch surfaceGrass;
    public AK.Wwise.Switch surfaceBush;

    public AK.Wwise.Switch walking;
    public AK.Wwise.Switch sneaking;
    public AK.Wwise.Switch land;

    //land
    bool isLanding = false;

    //Occlusion RTPC
    BaseEnemyScript[] ar_allEnemies;
    BaseEnemyScript[] ar_closeEnemies;
    public AK.Wwise.RTPC occlusionRTPC;
    public float maxHearingDistance;

    private void Awake()
    {
        sneakingStepIntervall = stepIntervall * 2;
        player = GetComponent<PlayerController>();
        m_cam = GetComponentInChildren<Camera>();

        ar_allEnemies = FindObjectsOfType<BaseEnemyScript>();

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
                    if (currUnderground == Underground.Bush)
                        volume = sInBushVolume;
                    else
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
                        case Underground.Bush:
                            volume = rVolume_Bush;
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


        //
        for (int i = 0; i < ar_allEnemies.Length; i++)
        {
            if (Vector3.Distance(ar_allEnemies[i].transform.position, this.transform.position) <= maxHearingDistance)
            {
                //OcclusionRaycast
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
            case 4: //bush
                surfaceBush.SetValue(this.gameObject);
                currUnderground = Underground.Bush;
                break;
            default: //stone
                surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;

        }
    }

    void CastOcclusionRays(Vector3 emitter_Pos)
    {
        RaycastHit hit;
        float hits = 0;

        //cast from player to emitter(enemy)
        Debug.DrawRay(this.transform.position, emitter_Pos - transform.position);
        if (Physics.Raycast(this.transform.position, emitter_Pos - transform.position, out hit, maxHearingDistance)) //, rayCastLayer
        {
            hits++;
        }
        //cast from player to leftside-emitter
        Vector3 leftsideEmitter = emitter_Pos - m_cam.transform.right;
        Debug.DrawRay(leftsideEmitter, transform.position - leftsideEmitter);
        if (Physics.Raycast(leftsideEmitter, transform.position - leftsideEmitter, out hit, maxHearingDistance))
        {
            hits++;
        }
        //cast from player to righthside-emitter
        Vector3 rightsideEmitter = emitter_Pos + m_cam.transform.right;
        Debug.DrawRay(rightsideEmitter, transform.position - rightsideEmitter);
        if (Physics.Raycast(rightsideEmitter, transform.position - rightsideEmitter, out hit, maxHearingDistance))
        {
            hits++;
        }

        //cast from leftside-player to emitter
        Vector3 leftsidePlayer = transform.position-m_cam.transform.right;
        Debug.DrawRay(leftsidePlayer, emitter_Pos - leftsidePlayer);
        if (Physics.Raycast(leftsidePlayer, emitter_Pos - leftsidePlayer, out hit, maxHearingDistance))
        {
            hits++;
        }
        //cast from leftside-player to leftside-emitter
        Debug.DrawRay(leftsidePlayer, leftsideEmitter - leftsidePlayer);
        if (Physics.Raycast(leftsidePlayer, leftsideEmitter - leftsidePlayer, out hit, maxHearingDistance))
        {
            hits++;
        }
        //cast from leftside-player to rightside-emitter
        Debug.DrawRay(leftsidePlayer, rightsideEmitter - leftsidePlayer);
        if (Physics.Raycast(leftsidePlayer, rightsideEmitter - leftsidePlayer, out hit, maxHearingDistance))
        {
            hits++;
        }

        //cast from rightside-player to emitter
        Vector3 rightsidePlayer = transform.position + m_cam.transform.right;
        Debug.DrawRay(rightsidePlayer, emitter_Pos - rightsidePlayer);
        if (Physics.Raycast(rightsidePlayer, emitter_Pos - rightsidePlayer, out hit, maxHearingDistance))
        {
            hits++;
        }
        //cast from rightside-player to leftside-emitter
        Debug.DrawRay(rightsidePlayer, leftsideEmitter - rightsidePlayer);
        if (Physics.Raycast(rightsidePlayer, leftsideEmitter - rightsidePlayer, out hit, maxHearingDistance))
        {
            hits++;
        }
        //cast from rightside-player to rightside-emitter
        Debug.DrawRay(rightsidePlayer, rightsideEmitter - rightsidePlayer);
        if (Physics.Raycast(rightsidePlayer, rightsideEmitter - rightsidePlayer, out hit, maxHearingDistance))
        {
            hits++;
        }

        print(Time.time + "; " + hits);
    }

    private void OnTriggerStay(Collider other)
    {
        //CastOcclusionRays
        if (other.CompareTag("Enemy"))
        {
            CastOcclusionRays(other.transform.position);
        }
    }
}
