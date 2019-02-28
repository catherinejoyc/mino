using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IHittable {

    Rigidbody m_rb;
    GameObject m_go;
    public Camera m_cam;

    //checkpoint (nicht mehr nötig)
    //public Vector3 m_lastCheckpoint;

    Vector3 m_playerInput;

    public float walkingSpeed;
    public float sneakingSpeed;
    public float jumpSpeed;
    public float inAirGravity;
    public float cameraSensitivity;

    //groundcheck
    bool m_isGrounded;
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

    //toxic water and ladders
    public GameObject waterBorder = null;

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
        if (!isPushingBox) //if not pushing something
        {
            m_go.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime);

            float mouseInput = Input.GetAxis("Mouse Y");

            float maxAngle = 20;
            if ((mouseInput > 0 && alpha < maxAngle) || (mouseInput < 0 && alpha > -maxAngle))
            {
                m_cam.transform.Rotate(Vector3.right * -mouseInput * cameraSensitivity * Time.deltaTime);
            }
        }
        #endregion

        #region Shoot Stones
        //Shoot Stones
        if (Input.GetButtonDown("Fire2"))
        {
            stone_startTime = Time.time;
        }
        if (Input.GetButtonUp("Fire2"))
        {
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
        if (/*Input.GetMouseButtonDown(0) || */Input.GetButton("Fire1"))
        {
            // currently empty-handed
            if (currentObjectHolding == null)
            {
                //[old] PickUp();
                Push();
            }
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
        m_playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Groundcheck
        if (Physics.CheckSphere(new Vector3(m_rb.position.x, m_rb.position.y - 0.65f,m_rb.transform.position.z), 0.45f, groundLayer))
        {
            //is grounded
            m_isGrounded = true;
        }
        else
        {
            m_isGrounded = false;
        }

        //Run and sneak
        if (Input.GetButton("Sneaking") || isPushingBox)
            m_rb.AddRelativeForce(m_playerInput * sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
        else
            m_rb.AddRelativeForce(m_playerInput * walkingSpeed * Time.deltaTime, ForceMode.Impulse);

        //Jump
        if (Input.GetButtonDown("Jump") && m_isGrounded)
            m_rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
        
        //Gravity in air
        if (!m_isGrounded)
        {
            m_rb.AddForce(new Vector3(0, inAirGravity, 0));
        }


        //Sound
        //je schneller desto lauter
    }

    private void OnTriggerEnter(Collider other)
    {
        ////Toxic water and ladders
        //if (other.CompareTag("WaterBorder"))
        //{
        //    waterBorder = other.gameObject;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        ////Toxic water and ladders
        //if (other.CompareTag("WaterBorder"))
        //{
        //    waterBorder = null;
        //}
    }

    #region Shoot Stone
    private void ShootStone()
    {
        if (InventoryManager.MyInstance.Stones >= 1)
        {
            Instantiate(pref_stone, transform.position + transform.forward * 0.56f + transform.right * 0.2f + transform.up * 0.5f, transform.rotation);
            InventoryManager.MyInstance.Stones--;
        }
    }

    private void CollectStone()
    {
        InventoryManager.MyInstance.Stones++;
    }
    #endregion

    #region Interact
    void Push()
    {
        //push with hands against box (animate)

        //push/pull box in facing direction
        RaycastHit hit;
        if (Physics.Raycast(transform.position /*+ transform.up * 0.5f*/, m_cam.transform.TransformVector(Vector3.forward), out hit, 0.5f))
        {
            if (hit.collider.gameObject.CompareTag("Box"))
            {
                currentObjectHolding = hit.collider.gameObject;

                //parent box to player and hold it in front of you
                currentObjectHolding.transform.parent = this.transform;
                currentObjectHolding.transform.Translate(0, 0, 0.2f * Time.deltaTime, this.gameObject.transform);
                //currentObjectHolding.GetComponent<Rigidbody>().isKinematic = true;
                currentObjectHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                //slow player
                isPushingBox = true;
            }
        }

        //PlayEvent
    }

    void StopPushing()
    {
        //hide hands again (animate)

        //de-parent box
        //currentObjectHolding.GetComponent<Rigidbody>().isKinematic = false;
        currentObjectHolding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currentObjectHolding.transform.parent = null;
        currentObjectHolding = null;
        //de-slow player
        isPushingBox = false;

        //StopEvent
    }

    #region [old] pick up box
    //void PickUp()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position + transform.up * 0.5f, m_cam.transform.TransformVector(Vector3.forward), out hit, maxDistanceToWall))
    //    {
    //        // pick up
    //        if (hit.collider.gameObject.CompareTag("Ladder") || hit.collider.gameObject.CompareTag("Box"))
    //        {
    //            currentObjectHolding = hit.collider.gameObject;

    //            //currentObjectHolding.SetActive(false);

    //            // hold object in hand
    //            currentObjectHolding.transform.parent = this.transform;
    //            currentObjectHolding.GetComponent<Rigidbody>().useGravity = false;                
    //        }
    //    }
    //}
    //void Place()
    //{
    //    // Place box infront of you
    //    //currentObjectHolding.transform.position = transform.position + transform.forward;
    //    //currentObjectHolding.SetActive(true);

    //    currentObjectHolding.transform.parent = null;
    //    currentObjectHolding.GetComponent<Rigidbody>().useGravity = true;
    //    currentObjectHolding = null;
    //}
    #endregion
    #endregion

    public void ReactToHit()
    {
        //Spawn on last checkpoint
        //transform.position = m_lastCheckpoint;

        //Reset level
        GameManager.MyInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    //public void SetCheckpoint(Vector3 _pos)
    //{
    //    m_lastCheckpoint = _pos;
    //}
}
