using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    static private InventoryManager myInstance;
    static public InventoryManager MyInstance
    {
        get
        {
            return myInstance;
        }
    }

    private void Awake()
    {
        if (myInstance == null)
            myInstance = this;
        else
            Debug.Log("InventoryManager already exists!");
    }

    public int Stones;
    
}
