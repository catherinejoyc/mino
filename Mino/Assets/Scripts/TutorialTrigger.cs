using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTrigger : MonoBehaviour {

    public Sprite TutorialScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {

            UIManager.MyInstance.TutorialScreen.sprite = TutorialScreen;
            UIManager.MyInstance.TutorialScreen.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            UIManager.MyInstance.TutorialScreen.enabled = false;
    }
}
