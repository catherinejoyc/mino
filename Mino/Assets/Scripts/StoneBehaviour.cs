using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneBehaviour : SoundScript {

    public Rigidbody m_rb;
    float m_startTime;

    private void Awake()
    {
        m_startTime = Time.time;
    }

    void FixedUpdate () {
        if (Time.time < m_startTime + 0.1f)
            m_rb.AddRelativeForce(new Vector3(0, 1f, 1f), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Player") //ignore player...
        {
            //if other underground
            if (collision.collider.GetComponent<UndergroundSound>() != null)
            {
                if (collision.collider.GetComponent<UndergroundSound>().UnderGroundIndex != 3) //if any other ground than grass
                {
                    m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
                    m_SoundEvent.RemoveAllListeners();
                    Invoke("Die", 1);
                }
            }
            else
            {
                m_SoundEvent.Invoke(this.transform.position, m_maxDistance);
                m_SoundEvent.RemoveAllListeners();
                Invoke("Die", 1);
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
