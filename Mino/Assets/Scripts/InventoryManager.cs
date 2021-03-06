﻿using System.Collections;
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

    private int stones;
    public int Stones
    {
        get
        {
            return stones;
        }
        set
        {
            Debug.Log("stones " + value);
            if (value <= 3)
            {
                stones = value;

                //CHANGE NUMBER SPRITE
                UIManager.MyInstance.AddStone(stones);
                //UIManager.MyInstance.stonecount.text = stones.ToString();
            }
        }
    }
}
