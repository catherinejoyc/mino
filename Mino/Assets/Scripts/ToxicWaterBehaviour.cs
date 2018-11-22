using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWaterBehaviour : MonoBehaviour {

    //see more in WaterBorderBehaviour

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerController>().Die();
        }
    }

}
