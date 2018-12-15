using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float timeOutInSeconds; //in sec

    private static GameManager m_myInstance;
    public static GameManager MyInstance
    {
        get
        {
            return m_myInstance;
        }
    }

    private void Awake()
    {
        if (MyInstance == null)
            m_myInstance = this;
        else
            Debug.Log("GameManager already exists!");
    }

    private void Update()
    {
        //Time Countdown
        float remainingTime = timeOutInSeconds - Time.time;
        UIManager.MyInstance.time.text = remainingTime.ToString("0");

        if (remainingTime <= 0)
            GameOver();

    }

    void GameOver() //Event drauß machen?
    {
        print("GameOver!");
    }
}
