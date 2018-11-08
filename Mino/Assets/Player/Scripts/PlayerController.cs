using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody m_rb;
    Vector3 m_playerInput;
    public float m_walkingSpeed = 25f;
    public float m_sneakingSpeed = 10f;
    public float m_jumpSpeed = 10f;

    bool m_isGrounded;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        m_playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Laufen und Schleichen
        if (Input.GetKey(KeyCode.LeftShift))
            m_rb.AddForce(m_playerInput * m_sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
        else
            m_rb.AddForce(m_playerInput * m_walkingSpeed * Time.deltaTime, ForceMode.Impulse);

        //Springen
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
            m_rb.AddForce(new Vector3(0, m_jumpSpeed, 0), ForceMode.Impulse);
   
    }

    private void OnTriggerEnter(Collider other)
    {
        m_isGrounded = true;

        //Rigidbody
        m_rb.mass = 1;
        m_rb.drag = 2.5f;
        //Movement
        m_walkingSpeed = 25f;
        m_sneakingSpeed = 10f;
        m_jumpSpeed = 10f;

    }

    private void OnTriggerExit(Collider other)
    {
        m_isGrounded = false;

        //Rigidbody
        m_rb.drag = 0f;
        //Movement
        m_walkingSpeed = 5f;
        m_sneakingSpeed = 1f;
        m_jumpSpeed = 1f;
    }
}
