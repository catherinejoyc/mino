using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : SoundScript {

    public PressureDoor door;

    [Tooltip("Particle Script (optional)")]
    public ParticleScript particleScript;

    int m_activePressurePoints = 0;

    public LayerMask activator;
    bool active = false;

    private void Update()
    {
        //activate
        if (Physics.CheckSphere(this.transform.position, 0.45f, activator))
        {
            Collider[] hits = Physics.OverlapSphere(this.transform.position, 0.45f, activator);
            foreach (Collider coll in hits)
            {
                print(coll.name);
            }
            if (!active)
            {
                door.UnlockDoor();

                //start secParticleSys
                if (particleScript != null)
                    particleScript.StartSecParticleSys();

                //Post Sound PlayEvent
                m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

                active = true;
            }
        }
        else //inactive
        {
            if (active)
            {
                door.CloseDoor();
                //stop secParticleSys
                if (particleScript != null)
                    particleScript.StopSecParticleSys();

                active = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.CompareTag("Box") || collision.gameObject.name == "KistenCollider")
        {
            m_activePressurePoints++;
            print(m_activePressurePoints);
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
        if (collision.gameObject.name == "Player" || collision.gameObject.CompareTag("Box") || collision.gameObject.name == "KistenCollider")
        {
            m_activePressurePoints--;
            print(m_activePressurePoints);
            if (m_activePressurePoints == 0)
            {
                door.CloseDoor();
                //stop secParticleSys
                if (particleScript != null)
                    particleScript.StopSecParticleSys();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.45f);
    }
}
