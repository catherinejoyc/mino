using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundSound : MonoBehaviour {

    GameObject m_Player;
    [Header("1 = Stone, 2 = Gravel, 3 = Grass, 4 = Bush")]
    [Tooltip("1 = Stone, 2 = Gravel, 3 = Grass, 4 = Bush")]
    public int UnderGroundIndex;

    private void Awake()
    {
        try
        {
            m_Player = FindObjectOfType<PlayerSoundScript>().gameObject;
        }
        catch
        {
            Debug.LogError ("No Player found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if (!other.isTrigger) //ignore the trigger
            {
                other.GetComponent<EnemySoundScript>().ChangeFootstep(UnderGroundIndex);
            }
        }
        else if (other.CompareTag("Box"))
        {
            other.GetComponent<BoxScript>().ChangeMovingSound(UnderGroundIndex);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerSoundScript>() != null)
        {
            m_Player.GetComponent<PlayerSoundScript>().ChangeFootstep(UnderGroundIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerSoundScript>() != null)
        {
            m_Player.GetComponent<PlayerSoundScript>().ChangeFootstep(0);
        }
        if(other.CompareTag("Enemy"))
        {
            if (!other.isTrigger) //ignore the trigger
                other.GetComponent<EnemySoundScript>().ChangeFootstep(0);
        }
    }
}
