using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //INGAME
    public GameObject ingameUI;
    public Image TutorialScreen;
    public Slider VolumeIndicator; //Audio!!!
    public Text keyFragments;
    public Text stonecount;
    public Text time;

    private static UIManager m_myInstance;
    public static UIManager MyInstance
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
            Debug.Log("UIManager already exists!");

    }

    public void ResetUI()
    {
        keyFragments.enabled = false;
        TutorialScreen.enabled = false;
    }

    //PAUSE
    public GameObject pauseUI;

    //DEATH
    public GameObject deathScreen;
    
}
