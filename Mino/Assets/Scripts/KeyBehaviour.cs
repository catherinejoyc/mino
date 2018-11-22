using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour {

    public GameObject[] keyfragments = new GameObject[3];
    public DoorBehaviour door;
    public int keyCount = 0;

    public void AddKeyCount()
    {
        keyCount++;

        if(keyCount == 3)
        {
            //activate door
            door.UnlockDoor();
        }
    }
}
