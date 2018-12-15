using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Rigidbody m_rb;
    GameObject m_go;
    Camera m_cam;

    //checkpoint
    public Vector3 m_lastCheckpoint;

    Vector3 m_playerInput;

    public float walkingSpeed = 25f;
    public float sneakingSpeed = 10f;
    public float jumpSpeed = 10f;
    public float inAirGravity = -25f;
    public float cameraSensitivity = 100f;

    //groundcheck
    bool m_isGrounded;
    public LayerMask groundLayer;
    //public LayerMask ground;

    //stone related
    public GameObject pref_stone;
    float stone_startTime = 0;

    //chalk related
    public GameObject sprite_chalk;
    public float maxDistanceToWall;
    float chalk_startTime = 0;

    //interact
    public GameObject currentObjectHolding = null;

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
        m_lastCheckpoint = transform.position;
    }

    private void Update()
    {
        #region Move Camera
        //Look around
        m_go.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime, 0));


        if (m_cam.transform.rotation.x > -0.10f && m_cam.transform.rotation.x < 0.25f)
            m_cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime, 0));
        if (m_cam.transform.rotation.x <= -0.10f) //if stopping point is reached
        {
            //only rotate downwards
            if (-Input.GetAxis("Mouse Y") > 0)
                m_cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime, 0));
        }
        else if (m_cam.transform.rotation.x >= 0.25f)
        {
            //only rotate upwards
            if (-Input.GetAxis("Mouse Y") < 0)
                m_cam.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime, 0));
        }
        //untere Grenze 0.25
        //obere Grenze -0.10
        #endregion

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
                ShootStone();
            }
            else
            {
                CollectStone(); //collect stones
            }
        }
        #endregion

        #region Chalk
        //Use Chalk
        if (Input.GetKeyDown(KeyCode.Q))
        {
            chalk_startTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (currentObjectHolding == null)
            {
                if ((Time.time - chalk_startTime) < 0.2f) //draw x if the player pressed the button for a short time
                {
                    DrawX();
                }
                else
                {
                    //delete X
                    DeleteX();
                }
            }
            else
                Debug.Log("Can't draw while holding something!");
        }
        #endregion

        #region Interact
        if (Input.GetMouseButtonDown(0))
        {
            // currently empty-handed
            if (currentObjectHolding == null)
            {
                PickUp();
            }
            else
            {
                Place();
            }
        }

        // hold object in front of player
        if (currentObjectHolding != null)
        {
            currentObjectHolding.transform.position = Vector3.MoveTowards(currentObjectHolding.transform.position, transform.position + transform.forward * 1.5f, Time.deltaTime * 5f);
            currentObjectHolding.transform.rotation = this.transform.rotation;
        }            
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
        if (Input.GetKey(KeyCode.LeftShift))
            m_rb.AddRelativeForce(m_playerInput * sneakingSpeed * Time.deltaTime, ForceMode.Impulse);
        else
            m_rb.AddRelativeForce(m_playerInput * walkingSpeed * Time.deltaTime, ForceMode.Impulse);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
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
        //Toxic water and ladders
        if (other.CompareTag("WaterBorder"))
        {
            waterBorder = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Toxic water and ladders
        if (other.CompareTag("WaterBorder"))
        {
            waterBorder = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Enemy contact
        if (collision.collider.CompareTag("Enemy"))
        {
            Die();
        }
    }

    #region Shoot Stone
    private void ShootStone()
    {
        if (InventoryManager.MyInstance.stones >= 1)
        {
            Instantiate(pref_stone, transform.position + transform.forward * 0.56f + transform.right * 0.2f + transform.up * 0.5f, transform.rotation);
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
        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_cam.transform.TransformVector(Vector3.forward), out hit, maxDistanceToWall))
        {
            // Add Sprite on wall
            if (!hit.collider.gameObject.CompareTag("x"))
            {
                GameObject x = Instantiate(sprite_chalk, hit.point, Quaternion.LookRotation(-hit.normal));
                x.transform.rotation = hit.transform.rotation;
            }
            else
                Debug.Log("You can't draw here!");
        }
        else
            Debug.Log("No wall detected. Try going closer.");
    }

    private void DeleteX()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_cam.transform.TransformVector(Vector3.forward), out hit, maxDistanceToWall))
        {
            Debug.Log(hit.collider.name);
            // delete X
            if (hit.collider.gameObject.CompareTag("x"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    #endregion

    #region Interact
    void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, m_cam.transform.TransformVector(Vector3.forward), out hit, maxDistanceToWall))
        {
            // pick up
            if (hit.collider.gameObject.CompareTag("Ladder") || hit.collider.gameObject.CompareTag("Box"))
            {
                currentObjectHolding = hit.collider.gameObject;

                //currentObjectHolding.SetActive(false);

                // hold object in hand
                currentObjectHolding.transform.parent = this.transform;
                currentObjectHolding.GetComponent<Rigidbody>().useGravity = false;                
            }
        }
    }

    void Place()
    {
        // ladder + water
        if (currentObjectHolding.CompareTag("Ladder"))
        {
            // Place ladder over river if possible
            if (waterBorder != null)
            {
                GameObject ladder = waterBorder.GetComponent<WaterBorderBehaviour>().ActivateLadder();

                // place ladder correctly
                ladder.transform.position = new Vector3(transform.position.x, ladder.transform.position.y, ladder.transform.position.z);

                // destroy ladder in hand
                currentObjectHolding.transform.parent = null;
                currentObjectHolding.SetActive(false);
                currentObjectHolding = null;
            }
            else
                Debug.Log("Can't place ladder here!");
        }
        else
        {
            // Place box infront of you
            //currentObjectHolding.transform.position = transform.position + transform.forward;
            //currentObjectHolding.SetActive(true);

            currentObjectHolding.transform.parent = null;
            currentObjectHolding.GetComponent<Rigidbody>().useGravity = true;
            currentObjectHolding = null;
        }
    }
    #endregion

    public void Die()
    {
        //Spawn on last checkpoint
        transform.position = m_lastCheckpoint;

        //Reset level
        //SceneManager.LoadScene();
    }

    public void SetCheckpoint(Vector3 _pos)
    {
        m_lastCheckpoint = _pos;
    }
}
