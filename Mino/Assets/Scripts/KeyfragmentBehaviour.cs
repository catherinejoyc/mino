using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyfragmentBehaviour : MonoBehaviour {

    public KeyBehaviour masterKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            //Update keycount
            masterKey.AddKeyCount();

            //dissapear
            gameObject.SetActive(false);
        }
    }
}
