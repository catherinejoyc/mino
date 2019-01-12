using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundSound : MonoBehaviour {

    GameObject m_Player;
    [Header("Choose UnderGroundIndex 1 = Stone, 2 = Gravel, 3 = Bush")]
    [Tooltip("1 = Stone, 2 = Gravel, 3 = Bush")]
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

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerSoundScript>() != null)
        {
            m_Player.GetComponent<PlayerSoundScript>().ChangeFootstep(UnderGroundIndex);
        }
        else if(other.GetComponent<EnemySoundScript>() != null)
        {
            other.GetComponent<EnemySoundScript>().ChangeFootstep(UnderGroundIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerSoundScript>() != null)
        {
            m_Player.GetComponent<PlayerSoundScript>().ChangeFootstep(0);
        }
        else if (other.GetComponent<EnemySoundScript>() != null)
        {
            other.GetComponent<EnemySoundScript>().ChangeFootstep(UnderGroundIndex);
        }
    }
}
