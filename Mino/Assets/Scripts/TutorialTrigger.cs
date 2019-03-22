using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTrigger : MonoBehaviour {

    public Sprite TutorialScreen;
    bool m_active = false;
    //float coolDownTime = 2;
    //float coolDownStart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            //set Cooldownstart
            //coolDownStart = Time.time;

            UIManager.MyInstance.TutorialScreen.sprite = TutorialScreen;
            UIManager.MyInstance.TutorialScreen.enabled = true;
            m_active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.MyInstance.TutorialScreen.enabled = false;

        //deactivate Tutorial Trigger
        //this.gameObject.SetActive(false);
    }

    //private void Update()
    //{
        //if (m_active && Time.time > coolDownStart + coolDownTime)
        //{
        //    UIManager.MyInstance.TutorialScreen.enabled = false;

        //    //deactivate Tutorial Trigger
        //    this.gameObject.SetActive(false);
        //}
    //}
}
