using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSoundScript : MonoBehaviour {

    // https://docs.unity3d.com/ScriptReference/Events.UnityEvent.AddListener.html

    /* Laute Geräusche:
     * Laufen
     * Landen nach Springen
     * ---
     * Kisten/Leitern platzieren
     * Rascheln im Gebüsch/andere Pflanzen)
    */
    UnityEvent m_loudSound = new UnityEvent();
   

    /* Leise Geräusche:
     * Schleichen
     * ---
     * Steinchen aufprallen (von der Decke und vom Spieler geworfen)
    */
    UnityEvent m_quietSound = new UnityEvent();

    private void Update()
    {
        //play Footstep every frame

    }
}
