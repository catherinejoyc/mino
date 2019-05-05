using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTriggerEnter : MonoBehaviour {

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.name == "Player")
    //        this.gameObject.SetActive(false);
    //}

    void DisableThisGameObj()
    {
        this.gameObject.SetActive(false);
    }
}
