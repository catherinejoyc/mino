using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyFloatEvent : UnityEvent<Vector3> //vector3 is position of sound
{
}

public class SoundScript : MonoBehaviour {

    public MyFloatEvent m_SoundEvent;
    // Wwise Sound
    public AK.Wwise.Event sound;
    // Sound Type
    public SoundType m_soundType;
    public SoundPriority soundValues;
    //max hearing distance (for enemy)
    protected float m_maxDistance;

    private void Start()
    {
        Debug.Log("SoundScript implementiert; " + this.gameObject.name);
        m_SoundEvent.AddListener(PlaySound);

        // set value to maxDistance
        switch (m_soundType)
        {
            case SoundType.ActivatedPressurePlate:
                m_maxDistance = soundValues.activatedPressurePlate;
                break;
            case SoundType.DestroyedBox:
                m_maxDistance = soundValues.destroyedBox;
                break;
            case SoundType.HardStoneImpact:
                m_maxDistance = soundValues.hardStoneImpact;
                break;
            case SoundType.MovingBox:
                m_maxDistance = soundValues.movingBox;
                break;
            case SoundType.MovingBush:
                m_maxDistance = soundValues.movingBush;
                break;
            case SoundType.MovingDoor:
                m_maxDistance = soundValues.movingDoor;
                break;
            case SoundType.MovingOnGrass:
                m_maxDistance = soundValues.movingOnGrass;
                break;
            case SoundType.MovingOnGravel:
                m_maxDistance = soundValues.movingOnGravel;
                break;
            case SoundType.MovingOnStone:
                m_maxDistance = soundValues.movingOnStone;
                break;
            case SoundType.PickUpStone:
                m_maxDistance = soundValues.pickUpStone;
                break;
            case SoundType.Sneaking:
                m_maxDistance = soundValues.sneaking;
                break;
            case SoundType.SoftStoneImpact:
                m_maxDistance = soundValues.softStoneImpact;
                break;
            case SoundType.UseChalk:
                m_maxDistance = soundValues.useChalk;
                break;
            default:
                Debug.LogError("No SoundType chosen! " + this.gameObject.name);
                break;
        }
    }

    //private void Update()
    //every few steps
    //m_SoundEvent.Invoke(this.transform.position); //Invoke Event with position of this Sound

    public float stepIntervall;
    float sneakingStepIntervall;
    float lastStepTime;
    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //play Footstep every few sec
            if (Input.GetButton("Sneaking"))
            {
                if (Time.time > lastStepTime + sneakingStepIntervall)
                {
                    //[old] m_PlaySneakingFootstep.Invoke();
                    //m_SoundEvent.Invoke(this.transform.position);

                    lastStepTime = Time.time;
                }
            }
            else
            {
                if (Time.time > lastStepTime + stepIntervall)
                {
                    //[old] m_PlayRunningFootstep.Invoke();
                    //m_SoundEvent.Invoke(this.transform.position);

                    lastStepTime = Time.time;
                }
            }
        }
    }

    //Aufruf im Script mit m_SoundEvent.Invoke(this.transform.position)
    protected void PlaySound(Vector3 pos)
    {
        Debug.Log("Posting Sound @" + this.gameObject.name);
        sound.Post(this.gameObject);
        Debug.Log("Sound posted @" + this.gameObject.name);
    }
}
