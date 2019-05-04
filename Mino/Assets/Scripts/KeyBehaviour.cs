using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour {

    public GameObject[] keyfragments = new GameObject[3];
    public KeyDoorBehaviour door;
    public int keyCount = 0;

    public void AddKeyCount()
    {
        keyCount++;
        UIManager.MyInstance.keyFragments.enabled = true;
        UIManager.MyInstance.keyFragments.text = keyCount.ToString() + "/3";

        if(keyCount == 3)
        {
            UIManager.MyInstance.PlayKeyfragmentEffect02();

            //activate door
            door.UnlockDoor();
        }
        else
        {
            UIManager.MyInstance.PlayKeyfragmentEffect01();
        }
    }
}
