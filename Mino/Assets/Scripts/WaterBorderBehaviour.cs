using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBorderBehaviour : MonoBehaviour {

    //Notes
    /*
     * Über dem Wasser ist bereits eine Leiter (SetActive(false)), die dann aktiviert wird, sobald man eine Leiter "hinstellt"
    */

    public GameObject m_ladder;

    public void ActivateLadder()
    {
        m_ladder.SetActive(true);
    }
}
