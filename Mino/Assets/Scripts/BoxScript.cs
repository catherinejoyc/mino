using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour, IHittable {

    bool damaged = false;

    //Materials
    Renderer m_renderer;
    MeshFilter m_meshFilter;
    public Material scratchedMat;
    public Mesh destroyedMesh;

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
            m_meshFilter.mesh = destroyedMesh;

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
