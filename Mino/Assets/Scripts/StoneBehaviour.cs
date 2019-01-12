using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneBehaviour : MonoBehaviour {

    public Rigidbody m_rb;
    float m_startTime;
    AudioSource m_audioSource;

    public UnityEvent m_playStoneSoundEvent = new UnityEvent();

    private void Awake()
    {
        m_startTime = Time.time;
        m_audioSource = GetComponent<AudioSource>();

        m_playStoneSoundEvent.AddListener(PlaySound);
    }

    void FixedUpdate () {
        if (Time.time < m_startTime + 0.1f)
            m_rb.AddRelativeForce(new Vector3(0, 1f, 1f), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Player")
        {
            print(collision.collider.name);
            m_playStoneSoundEvent.Invoke();
            m_playStoneSoundEvent.RemoveAllListeners();
            Invoke("Die", 1);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void PlaySound()
    {
        m_audioSource.Play();
    }
}
