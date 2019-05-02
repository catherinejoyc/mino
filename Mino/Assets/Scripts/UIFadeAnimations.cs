using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeAnimations : MonoBehaviour {

    //Animator
    public Animator buttonPressed_Animator;

    void ButtonPressAnimation()
    {
        //play fade in StartPressed Image
        buttonPressed_Animator.SetTrigger("startPressed");

        
    }
}
