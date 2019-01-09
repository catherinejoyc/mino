using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkBehaviour : MonoBehaviour {

    public ChalkScript chalkScript;
    public PressureDoor door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            chalkScript.enabled = true;

            door.UnlockDoor();

            this.gameObject.SetActive(false);
        }
    }
}
