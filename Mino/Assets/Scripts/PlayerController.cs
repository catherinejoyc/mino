﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IHittable {

    Rigidbody m_rb;
    GameObject m_go;
    public Camera m_cam;

    //checkpoint (nicht mehr nötig)
    //public Vector3 m_lastCheckpoint;

    //moving
    Vector3 m_playerInput;
    bool canMove = true;

    public float walkingSpeed;
    public float sneakingSpeed;
    public float boxMovingSpeed;
    public float jumpSpeed;
    public float inAirGravity;
    public float cameraSensitivity;

    //groundcheck
    public bool m_isGrounded;
    public LayerMask groundLayer;
    //public LayerMask ground;

    //stone related
    public GameObject pref_stone;
    float stone_startTime = 0;

    //chalk related
    //public GameObject sprite_chalk;
    public float maxDistanceToWall;
    //float chalk_startTime = 0;

    //interact
    public GameObject currentObjectHolding = null;
    public bool isPushingBox = false;
    bool boxPathIsBlocked;

    //toxic water and ladders
    public GameObject waterBorder = null;

    //Animator
    public Animator playerAnimator;

    //KistenCollider
    public GameObject kistenCollider;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_go = this.gameObject;
        m_cam = GetComponentInChildren<Camera>();

        Cursor.visible = false;
    }

    private void Start()
    {
        //m_lastCheckpoint = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        #region Move Camera


        float gk = m_cam.transform.forward.y;
        float alpha = -(Mathf.Acos(gk) * Mathf.Rad2Deg - 90);

        //Look around
        //if (!isPushingBox) //if not pushing something
        //{
        float mouseInputX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseInputX) >= 0.1)
            m_go.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime);

            float mouseInputY = Input.GetAxis("Mouse Y");
        if (Mathf.Abs(mouseInputY) >= 0.1)
        {
            print("rotate");
            float maxAngle = 20;
            if ((mouseInputY > 0 && alpha < maxAngle) || (mouseInputY < 0 && alpha > -maxAngle))
            {
                m_cam.transform.Rotate(Vector3.right * -mouseInputY * cameraSensitivity * Time.deltaTime);
            }
        }


        //}

        #endregion

        #region Shoot Stones
        //Shoot Stones
        if (Input.GetButtonDown("Fire2"))
        {
            //stop movement and get down
            canMove = false;
            //GetComponentInChildren<Camera>().gameObject.transform.Translate(-Vector3.up * 0.1f);

            stone_startTime = Time.time;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            //enable movement and go back up
            canMove = true;
            //GetComponentInChildren<Camera>().gameObject.transform.Translate(Vector3.up * 0.1f);

            if ((Time.time - stone_startTime) < 0.2f)  //shoot if the player pressed the button for a short time
            {
                ShootStone();
            }
            else
            {
                CollectStone(); //collect stones
            }
        }
        #endregion

        #region Interact
        if (/*Input.GetMouseButtonDown(0) || */Input.GetButton("Fire1") && m_isGrounded) //only if player is grounded
        {
            // currently empty-handed
            if (currentObjectHolding == null)
            {           
                Push();
            }
            //else
            //{
            //    //check path in front of box
            //    //https://stackoverflow.com/questions/24563085/raycast-but-ignore-yourself
            //    //https://docs.unity3d.com/ScriptReference/Physics.RaycastAll.html

            //    //check every hit
            //    RaycastHit[] hits;
            //    Vector3 playerFrwd = m_cam.transform.TransformDirection(Vector3.forward);

            //    hits = Physics.RaycastAll(currentObjectHolding.transform.position, playerFrwd, 0.5f);
            //    for (int i = 0; i < hits.Length; i++)
            //    {
            //        RaycastHit hit = hits[i];
            //        if (!hit.collider.CompareTag("Box") && !hit.collider.isTrigger) //if hit anything other than the box itself
            //        {
            //            boxPathIsBlocked = true;
            //            break;
            //        }
            //        else
            //            boxPathIsBlocked = false;
            //    }

            //    //Vector3 playerFrwd = m_cam.transform.TransformDirection(Vector3.forward) * 0.1f;
            //    Debug.DrawRay(currentObjectHolding.transform.position, playerFrwd, Color.cyan, 0.1f);
            //    //if (Physics.Raycast(currentObjectHolding.transform.position, playerFrwd))
            //    //{
            //    //    //stop Movement forward
            //    //    boxPathIsBlocked = true;
            //    //}
            //    //else
            //    //    boxPathIsBlocked = false;
            //}
        }
        else if (currentObjectHolding != null)
        {
            //[old] Place();
            StopPushing();
        }

        //// hold object in front of player
        //if (currentObjectHolding != null)
        //{
        //    currentObjectHolding.transform.position = Vector3.MoveTowards(currentObjectHolding.transform.position, transform.position + transform.forward * 1.5f, Time.deltaTime * 5f);
        //    currentObjectHolding.transform.rotation = this.transform.rotation;
        //}
        #endregion
    }

    void FixedUpdate () {

        ////m_playerInput
        //if (isPushingBox) //if pushing a box, only move forward and backwards
        //{
        //    if (Input.GetAxis("Vertical") > 0) //if input is positive (forward)
        //    {
        //        if (boxPathIsBlocked) //not moving if path is blocked
        //            m_playerInput = new Vector3(0, 0, 0);
        //        else
        //            m_playerInput = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //    }
        //    else if (Input.GetAxis("Vertical") < 0) //if input is negative (backwards)
        //    {
        //        m_playerInput = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //    }

        //    //if (boxPathIsBlocked) //if path is blocked, only move backwards
        //    //{
        //    //    if (Input.GetAxis("Vertical") > 0)
        //    //    {
        //    //        m_playerInput = new Vector3(0, 0, 0);
        //    //    }
        //    //    if (Input.GetAxis("Vertical") < 0)
        //    //    {          
        //    //        m_playerInput = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //    //    }                   
        //    //}
        //    //else
        //    //{
        //    //    m_playerInput = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //    //}

            
        //}
        //else
        m_playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Groundcheck
        if (Physics.CheckSphere(new Vector3(m_rb.position.x, m_rb.position.y - 0.5f,m_rb.transform.position.z), 0.2f, groundLayer))
        {
            //is grounded
            m_isGrounded = true;
        }
        else
        {
            m_isGrounded = false;
        }

        //Run and sneak
        if (canMove)
        {
            if (isPushingBox)
            {
                if (Input.GetButton("Sneaking") || Input.GetAxis("Sneaking") > 0)
                    m_rb.AddRelativeForce(m_playerInput * sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
                else
                    m_rb.AddRelativeForce(m_playerInput * boxMovingSpeed * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                if (Input.GetButton("Sneaking") || Input.GetAxis("Sneaking") > 0)
                    m_rb.AddRelativeForce(m_playerInput * sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
                else
                    m_rb.AddRelativeForce(m_playerInput * walkingSpeed * Time.deltaTime, ForceMode.Impulse);
            }
        }

        //Jump
        if (Input.GetButtonDown("Jump") && m_isGrounded && !isPushingBox)
        {
            //Jump
            m_rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
        }
        
        //Gravity in air
        if (!m_isGrounded)
        {
            m_rb.AddForce(new Vector3(0, inAirGravity, 0));
        }


        //Sound
        //je schneller desto lauter
    }

    #region Shoot Stone
    public Animator mainCamAnimator;
    private void ShootStone()
    {
        if (InventoryManager.MyInstance.Stones >= 1)
        {
            //throw (animate)
            playerAnimator.SetTrigger("throwStone");

            Instantiate(pref_stone, transform.position + transform.forward * 0.56f + transform.right * 0.2f + transform.up * 0.5f, transform.rotation);
            InventoryManager.MyInstance.Stones--;
        }
    }

    private void CollectStone()
    {
        if (GetComponent<PlayerSoundScript>().currUnderground == Underground.Gravel)
        {
            //kneel down (animate cam)
            mainCamAnimator.SetTrigger("collectStone");

            InventoryManager.MyInstance.Stones++;
        }
    }
    #endregion

    #region Interact
    float _boxXOffset = 0;
    float _boxYOffset = 0;
    public float _boxZOffest; //0.1

    void Push()
    {
        //push/pull box in facing direction
        RaycastHit hit;
        if (Physics.Raycast(transform.position /*+ transform.up * 0.5f*/, m_cam.transform.TransformVector(Vector3.forward), out hit, 0.5f))
        {
            if (hit.collider.gameObject.CompareTag("Box"))
            {
                currentObjectHolding = hit.collider.gameObject;

                //push with hands against box (animate)
                playerAnimator.SetTrigger("pushBox");

                //freeze rotations
                currentObjectHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                //parent box to player and hold it in front of you
                //KISTENCOLLIDER - COLLIDER DER KISTE SELBST AUSSCHALTEN
                currentObjectHolding.GetComponent<Collider>().enabled = false;

                currentObjectHolding.transform.parent = this.transform;

                //snap box infront of player
                Vector3 currObj_transform = new Vector3(0, -0.6f, 1f);
                currentObjectHolding.transform.SetPositionAndRotation(transform.TransformPoint(currObj_transform), this.gameObject.transform.rotation);
                currentObjectHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

                //turn on KISTENCOLLIDER
                kistenCollider.SetActive(true);

                //slow player
                isPushingBox = true;
            }
        }

        //PlayEvent
    }

    void StopPushing()
    {
        //hide hands again (animate)
        playerAnimator.SetTrigger("hideHands");

        //de-parent box
        //KISTENCOLLIDER - COLLIDER DER KISTE SELBST ANSCHALTEN
        currentObjectHolding.GetComponent<Collider>().enabled = true;

        currentObjectHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currentObjectHolding.transform.parent = null;
        currentObjectHolding = null;

        //turn on KISTENCOLLIDER
        kistenCollider.SetActive(false);

        //de-slow player
        isPushingBox = false;
        //de block path
        boxPathIsBlocked = false;


        //StopEvent
    }
    #endregion

    public void ReactToHit()
    {
        UIManager.MyInstance.ShowDeathScreen();
    }
}
