using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBehaviour : MonoBehaviour {

    public Rigidbody m_rb;
    float m_startTime;
    AudioSource m_audioSource;

    private void Awake()
    {
        m_startTime = Time.time;
        m_audioSource = GetComponent<AudioSource>();
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
            m_audioSource.Play();
            Invoke("Die", 1);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
