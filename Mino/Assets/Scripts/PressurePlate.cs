using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : SoundScript {

    public PressureDoor door;

    [Tooltip("Particle Script (optional)")]
    public ParticleScript particleScript;

    int m_activePressurePoints = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.CompareTag("Box"))
        {
            m_activePressurePoints++;
            door.UnlockDoor();

            //start secParticleSys
            if (particleScript != null)
                particleScript.StartSecParticleSys();

            //Post Sound PlayEvent
            m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.CompareTag("Box"))
        {
            m_activePressurePoints--;
            if (m_activePressurePoints == 0)
            {
                door.CloseDoor();
                //stop secParticleSys
                if (particleScript != null)
                    particleScript.StopSecParticleSys();
            }
        }
    }
}
