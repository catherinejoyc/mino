using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBorderBehaviour : MonoBehaviour {

    //Notes
    /*
     * Über dem Wasser ist bereits eine Leiter (SetActive(false)), die dann aktiviert wird, sobald man eine Leiter "hinstellt"
     * Diese Leiter ist perfekt auf dem Wasser platziert (z-Achse), kann aber entlang des Flusses variabel platziert sein (lokale x-Achse der Leiter). siehe PlayerController!
    */

    public GameObject m_ladder;

    public GameObject ActivateLadder()
    {
        m_ladder.SetActive(true);
        return m_ladder;
    }
}
