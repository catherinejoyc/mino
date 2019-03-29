using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Underground
{
    Stone,
    Gravel,
    Grass,
    Bush
}

public class PlayerSoundScript : SoundScript {
    //Events for sneaking and landing
    public AK.Wwise.Event sneakingPlayEvent;
    public AK.Wwise.Event landPlayEvent;
    public MyFloatEvent _sneakingEvent;
    public MyFloatEvent _landEvent;

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
    public Underground currUnderground;

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

    //land
    bool isLanding = false;

    //Occlusion RTPC
    public float maxHearingDistance;

    private void Awake()
    {
        sneakingStepIntervall = stepIntervall * 2;
        player = GetComponent<PlayerController>();
        m_cam = GetComponentInChildren<Camera>();

        //Add Play Sound to Listener
        _sneakingEvent.AddListener(PlaySneakingSound);
        _landEvent.AddListener(PlaySneakingSound);
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
            _landEvent.Invoke(this.transform.position, m_maxDistance);
            isLanding = false;
        }

        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && player.m_isGrounded)
        {
            //play Footstep every few sec
            if (Input.GetButton("Sneaking"))
            {
                if (Time.time > lastStepTime + sneakingStepIntervall)
                {
                    //update volume/soundType
                    if (currUnderground == Underground.Bush)
                    {
                        volume = sInBushVolume;
                        ChangeSoundType(SoundType.MovingOnGrass);
                    }
                    else
                    {
                        volume = sVolume;
                        ChangeSoundType(SoundType.Sneaking);
                    }

                    //m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
                    _sneakingEvent.Invoke(this.transform.position, m_maxDistance);

                    lastStepTime = Time.time;
                }
            }
            else
            {
                if (Time.time > lastStepTime + stepIntervall)
                {
                    //Check underground and update volume/soundType
                    switch (currUnderground)
                    {
                        case Underground.Stone:
                            volume = rVolume_Stone;
                            ChangeSoundType(SoundType.MovingOnStone);
                            break;
                        case Underground.Gravel:
                            volume = rVolume_Gravel;
                            ChangeSoundType(SoundType.MovingOnGravel);
                            break;
                        case Underground.Grass:
                            volume = rVolume_Grass;
                            ChangeSoundType(SoundType.MovingOnGrass);
                            break;
                        case Underground.Bush:
                            volume = rVolume_Bush;
                            ChangeSoundType(SoundType.MovingBush);
                            break;
                        default:
                            volume = rVolume_Stone;
                            ChangeSoundType(SoundType.MovingOnStone);
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

    void CastOcclusionRays(Vector3 emitter_Pos, EnemySoundScript enemy)
    {
        float hits = 0;

        //cast from player to emitter(enemy)
        Debug.DrawRay(this.transform.position, emitter_Pos - transform.position);
        if (Physics.Raycast(this.transform.position, emitter_Pos - transform.position, Vector3.Distance(emitter_Pos, transform.position), rayCastLayer)) //, rayCastLayer
            hits++;
        //cast from player to leftside-emitter
        Vector3 leftsideEmitter = emitter_Pos - m_cam.transform.right;
        Debug.DrawRay(leftsideEmitter, transform.position - leftsideEmitter);
        if (Physics.Raycast(leftsideEmitter, transform.position - leftsideEmitter, Vector3.Distance(leftsideEmitter, transform.position), rayCastLayer))
            hits++;
        //cast from player to righthside-emitter
        Vector3 rightsideEmitter = emitter_Pos + m_cam.transform.right;
        Debug.DrawRay(rightsideEmitter, transform.position - rightsideEmitter);
        if (Physics.Raycast(rightsideEmitter, transform.position - rightsideEmitter, Vector3.Distance(rightsideEmitter, transform.position), rayCastLayer))
            hits++;

        //cast from leftside-player to emitter
        Vector3 leftsidePlayer = transform.position-m_cam.transform.right;
        Debug.DrawRay(leftsidePlayer, emitter_Pos - leftsidePlayer);
        if (Physics.Raycast(leftsidePlayer, emitter_Pos - leftsidePlayer, Vector3.Distance(emitter_Pos, leftsidePlayer), rayCastLayer))
            hits++;
        //cast from leftside-player to leftside-emitter
        Debug.DrawRay(leftsidePlayer, leftsideEmitter - leftsidePlayer);
        if (Physics.Raycast(leftsidePlayer, leftsideEmitter - leftsidePlayer, Vector3.Distance(leftsideEmitter, leftsidePlayer), rayCastLayer))
            hits++;
        //cast from leftside-player to rightside-emitter
        Debug.DrawRay(leftsidePlayer, rightsideEmitter - leftsidePlayer);
        if (Physics.Raycast(leftsidePlayer, rightsideEmitter - leftsidePlayer, Vector3.Distance(rightsideEmitter, leftsidePlayer), rayCastLayer))
            hits++;

        //cast from rightside-player to emitter
        Vector3 rightsidePlayer = transform.position + m_cam.transform.right;
        Debug.DrawRay(rightsidePlayer, emitter_Pos - rightsidePlayer);
        if (Physics.Raycast(rightsidePlayer, emitter_Pos - rightsidePlayer, Vector3.Distance(emitter_Pos, rightsidePlayer), rayCastLayer))
            hits++;
        //cast from rightside-player to leftside-emitter
        Debug.DrawRay(rightsidePlayer, leftsideEmitter - rightsidePlayer);
        if (Physics.Raycast(rightsidePlayer, leftsideEmitter - rightsidePlayer, Vector3.Distance(leftsideEmitter, rightsidePlayer), rayCastLayer))
            hits++;
        //cast from rightside-player to rightside-emitter
        Debug.DrawRay(rightsidePlayer, rightsideEmitter - rightsidePlayer);
        if (Physics.Raycast(rightsidePlayer, rightsideEmitter - rightsidePlayer, Vector3.Distance(rightsideEmitter, rightsidePlayer), rayCastLayer))
            hits++;

        enemy.occlusionRTPC.SetValue(enemy.gameObject, hits);
    }

    private void OnTriggerStay(Collider other)
    {
        //CastOcclusionRays
        if (other.CompareTag("Enemy"))
        {
            CastOcclusionRays(other.transform.position, other.GetComponent<EnemySoundScript>());
        }
    }

    //PlaySoundEvents
    protected void PlaySneakingSound(Vector3 pos, float maxDistance)
    {
        sneakingPlayEvent.Post(this.gameObject);
        //Debug.Log("Sound posted @" + this.gameObject.name);
    }
    protected void PlayLandSound(Vector3 pos, float maxDistance)
    {
        landPlayEvent.Post(this.gameObject);
        //Debug.Log("Sound posted @" + this.gameObject.name);
    }
}
