using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSoundScript : MonoBehaviour {

    PlayerController m_player;
    public float stepIntervall;
    float lastStepTime;

    /* Laute Geräusche:
     * Laufen
     * Landen nach Springen
     * ---
     * Kisten/Leitern platzieren
     * Rascheln im Gebüsch/andere Pflanzen)
    */
    UnityEvent m_PlayRunningFootstep = new UnityEvent();
   

    /* Leise Geräusche:
     * Schleichen
     * ---
     * Steinchen aufprallen (von der Decke und vom Spieler geworfen)
    */
    UnityEvent m_PlaySneakingFootstep = new UnityEvent();

    private void Awake()
    {
        //Add PlayAudio to Events
        m_PlayRunningFootstep.AddListener(PlayRunningFootstep);
        m_PlaySneakingFootstep.AddListener(PlaySneakingFootstep);
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //play Footstep every few sec
            if (Time.time > lastStepTime + stepIntervall)
            {
                //Play Events
                m_PlayRunningFootstep.Invoke();

                lastStepTime = Time.time;
            }
        }
    }

    void PlayRunningFootstep()
    {
        print("Footstep");
    }
    void PlaySneakingFootstep()
    {
        print("Sneak");
    }
}
