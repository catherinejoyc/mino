using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    static InputManager myInstance;
    static InputManager MyInstance
    {
        get
        {
            return myInstance;
        }
    }

    Invoker activeInvoker;

    private void Awake()
    {
        if (myInstance == null)
            myInstance = this;
        else
            Debug.Log("InputManager already exists!");

        //Invoker
        activeInvoker = FindObjectOfType<KeyBoardPlayerController>();
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            //Run

        }
            

        //verticalInput = Input.GetAxis("Vertical");

        ////Run and sneak
        //if (Input.GetKey(KeyCode.LeftShift))
        //    m_rb.AddRelativeForce(m_playerInput * m_sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
        //else
        //    m_rb.AddRelativeForce(m_playerInput * m_walkingSpeed * Time.deltaTime, ForceMode.Impulse);

        ////Jump
        //if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        //    m_rb.AddForce(new Vector3(0, m_jumpSpeed, 0), ForceMode.Impulse);

        ////Gravity in air
        //if (!m_isGrounded)
        //{
        //    m_rb.AddForce(new Vector3(0, m_inAirGravity, 0));
        //}

        ////Look around
        //m_go.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * m_cameraSensitivity * Time.deltaTime, 0));
    }

}
