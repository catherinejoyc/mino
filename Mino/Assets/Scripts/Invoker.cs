using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Invoker
{
    
}

public class KeyBoardPlayerController : MonoBehaviour, Invoker {

    Rigidbody m_rb;
    GameObject m_go;

    Vector3 m_playerInput;

    public float m_walkingSpeed = 25f;
    public float m_sneakingSpeed = 10f;
    public float m_jumpSpeed = 10f;
    public float m_inAirGravity = -25f;
    public float m_cameraSensitivity = 20f;

    bool m_isGrounded;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_go = this.gameObject;
    }

    // Update is called once per frame
    void Update () {
        m_playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Run and sneak
        if (Input.GetKey(KeyCode.LeftShift))
            m_rb.AddRelativeForce(m_playerInput * m_sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
        else
            m_rb.AddRelativeForce(m_playerInput * m_walkingSpeed * Time.deltaTime, ForceMode.Impulse);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
            m_rb.AddForce(new Vector3(0, m_jumpSpeed, 0), ForceMode.Impulse);
        
        //Gravity in air
        if (!m_isGrounded)
        {
            m_rb.AddForce(new Vector3(0, m_inAirGravity, 0));
        }

        //Look around
        m_go.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * m_cameraSensitivity * Time.deltaTime, 0));

        //Shoot Stones

        //Sound
        //je schneller desto lauter
    }

    private void OnTriggerEnter(Collider other)
    {
        m_isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        m_isGrounded = false;
    }
}
