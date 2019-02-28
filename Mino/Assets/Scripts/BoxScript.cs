using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : SoundScript, IHittable {

    bool damaged = false;
    bool isMoving = false;

    //audio
    public AK.Wwise.Event stopSoundEvent;
    public AK.Wwise.Event ScratchPlayEvent;
    public AK.Wwise.Event DestroyedPlayEvent;

    //Materials
    Renderer m_renderer;
    MeshFilter m_meshFilter;
    public Material scratchedMat;
    public GameObject destroyedBox;

    private void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (GetComponentInParent<PlayerController>() != null && !isMoving) //check if player is currently holding box
        {
            StartMovingSound();
            isMoving = true;
        }
        else if (isMoving)
        {
            stopSoundEvent.Post(this.gameObject);
            isMoving = false;
        }
    }

    public void ReactToHit()
    {
        //if damaged, destroy itself
        if (damaged)
        {
            //play destroyed sound
            m_soundType = SoundType.DestroyedBox;
            DestroyedPlayEvent.Post(this.gameObject);

            //particle system (perfomance issues)

            //change mesh
            Instantiate(destroyedBox, this.transform);
            m_renderer.enabled = false;
            //change collider
            BoxCollider b = this.GetComponent<Collider>() as BoxCollider;
            if (b != null)
            {
                b.size = new Vector3(0.1f, 10f, 10f);
            }

            //Destroy(this.gameObject);
        }
        else
        {
            //play scratched sound
            m_soundType = SoundType.DestroyedBox;
            ScratchPlayEvent.Post(this.gameObject);

            //Change Normal Map + damaged
            m_renderer.material = scratchedMat;

            damaged = true;
        }
    }

    public void StartMovingSound()
    {
        m_soundType = SoundType.MovingBox;
        m_SoundEvent.Invoke(transform.position, m_maxDistance);
    }
}
