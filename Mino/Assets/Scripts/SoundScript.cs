using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundScript : MonoBehaviour {

    public UnityEvent m_SoundEvent = new UnityEvent();
    // Wwise Sound
    public AK.Wwise.Event sound;

    private void Start()
    {
        m_SoundEvent.AddListener(PlaySound);
    }

    private void Update()
    {
        //every few steps
        m_SoundEvent.Invoke();
    }

    void PlaySound()
    {
        sound.Post(this.gameObject);
    }
}
