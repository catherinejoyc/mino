using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBehaviour : MonoBehaviour {

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
        Destroy(gameObject);
    }
}
