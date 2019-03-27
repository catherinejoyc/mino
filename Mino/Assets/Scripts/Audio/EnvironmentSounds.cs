using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSounds : SoundScript {

    //One Play Event
    private void Awake()
    {
        m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
    }
}
