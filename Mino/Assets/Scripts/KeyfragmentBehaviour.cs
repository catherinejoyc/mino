using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyfragmentBehaviour : SoundScript {

    public KeyBehaviour masterKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            m_SoundEvent.Invoke(this.transform.position, m_maxDistance);

            //Update keycount
            masterKey.AddKeyCount();

            Invoke("Dissapear", 0.25f);
        }
    }

    //dissapear
    void Dissapear()
    {
        gameObject.SetActive(false);
    }
}
