using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    public PressureDoor door;

    int m_activePressurePoints = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.CompareTag("Box"))
        {
            m_activePressurePoints++;
            door.UnlockDoor();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.CompareTag("Box"))
        {
            m_activePressurePoints--;
            if (m_activePressurePoints == 0)
                door.CloseDoor();
        }
    }
}
