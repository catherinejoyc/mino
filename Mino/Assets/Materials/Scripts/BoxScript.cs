using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour, IHittable {

    bool damaged = false;

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

    public void ReactToHit()
    {
        //if damaged, destroy itself
        if (damaged)
        {
            //particle system

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
            //Change Normal Map + damaged
            m_renderer.material = scratchedMat;

            damaged = true;
        }
    }
}
