using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrahlScript : MonoBehaviour {

    public int endLevelIndex;

    private void OnTriggerEnter(Collider other)
    {
        //Endscreen
        SceneManager.LoadScene(endLevelIndex);
        
    }
}
