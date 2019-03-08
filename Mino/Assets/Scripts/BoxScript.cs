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

    public float movingBoxVol;

    //underground
    Underground currUnderground;
    [Header("Ak Switches")]//Switches
    public AK.Wwise.Switch surfaceStone;
    public AK.Wwise.Switch surfaceGravel;
    public AK.Wwise.Switch surfaceGrass;

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
        if (GetComponentInParent<PlayerController>() != null) //check if player is currently holding box
        {
            if (!isMoving)
            {
                StartMovingSound();
                isMoving = true;
            }
            UIManager.MyInstance.VolumeIndicator.value = movingBoxVol;
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
            ChangeSoundType(SoundType.DestroyedBox);
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
            ChangeSoundType(SoundType.DestroyedBox);
            ScratchPlayEvent.Post(this.gameObject);

            //Change Normal Map + damaged
            m_renderer.material = scratchedMat;

            damaged = true;
        }
    }

    public void StartMovingSound()
    {
        ChangeSoundType(SoundType.MovingBox);
        m_SoundEvent.Invoke(transform.position, m_maxDistance);
    }

    public void ChangeMovingSound(int underGround)
    {
        //set switches and currUnderground
        switch (underGround)
        {
            case 1: //stone
                surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;
            case 2: //gravel
                surfaceGravel.SetValue(this.gameObject);
                currUnderground = Underground.Gravel;
                break;
            case 3: //grass
                surfaceGrass.SetValue(this.gameObject);
                currUnderground = Underground.Grass;
                break;
            default: //stone
                surfaceStone.SetValue(this.gameObject);
                currUnderground = Underground.Stone;
                break;

        }
    }
}
