using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody m_rb;
    GameObject m_go;

    Vector3 m_playerInput;

    public float m_walkingSpeed = 25f;
    public float m_sneakingSpeed = 10f;
    public float m_jumpSpeed = 10f;
    public float m_inAirGravity = -25f;
    public float m_cameraSensitivity = 100f;

    bool m_isGrounded;

    //stone related
    public GameObject m_pref_stone;
    float stone_startTime = 0;

    //chalk related
    public GameObject sprite_chalk;
    public float maxDistanceToWall;
    float chalk_startTime = 0;
    public LayerMask notDrawable;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_go = this.gameObject;
    }

    private void Update()
    {
        //Look around
        m_go.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * m_cameraSensitivity * Time.deltaTime, 0));

        #region Shoot Stones
        //Shoot Stones
        if (Input.GetMouseButtonDown(1))
        {
            stone_startTime = Time.time;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if ((Time.time - stone_startTime) < 0.2f)  //shoot if the player pressed the button for a short time
            {
                Debug.Log("Shoot");
                ShootStone();
            }
            else
            {
                CollectStone(); //collect stones
                Debug.Log("Collect");
            }
        }
        #endregion

        //Use Chalk
        if (Input.GetKeyDown(KeyCode.Q))
        {
            chalk_startTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if ((Time.time - chalk_startTime) < 0.2f) //draw x if the player pressed the button for a short time
            {
                Debug.Log("draw X");
                DrawX();
            }
            else
            {
                //delete X
            }
        }
    }

    void FixedUpdate () {
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

    #region Shoot Stone
    private void ShootStone()
    {
        if (InventoryManager.MyInstance.stones >= 1)
        {
            Instantiate(m_pref_stone, transform.position + transform.forward * 0.56f + transform.right * 0.2f + transform.up * 0.5f, transform.rotation);
            InventoryManager.MyInstance.stones--;
        }
    }

    private void CollectStone()
    {
        InventoryManager.MyInstance.stones++;
    }
    #endregion

    #region Chalk
    private void DrawX()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.TransformDirection(Vector3.forward), out hit, maxDistanceToWall))
        {
            // Add Sprite on wall
            if (hit.collider.gameObject.layer != notDrawable)
            {
                GameObject x = Instantiate(sprite_chalk, hit.point, Quaternion.LookRotation(-hit.normal));
                x.transform.rotation = hit.transform.rotation;
            }
        }
    }
    #endregion
}
