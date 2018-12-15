using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTrigger : MonoBehaviour {

    public Sprite TutorialScreen;
    bool m_active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            print("show tutorial");
            UIManager.MyInstance.TutorialScreen.sprite = TutorialScreen;
            UIManager.MyInstance.TutorialScreen.enabled = true;
            m_active = true;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (m_active)
                UIManager.MyInstance.TutorialScreen.enabled = false;
        }
    }
}
