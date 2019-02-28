using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float timeOutInSeconds; //in sec


    PlayerSoundScript m_playerSound;


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


        m_playerSound = FindObjectOfType<PlayerSoundScript>();

    }

    private void Update()
    {
        //Time Countdown
        float remainingTime = timeOutInSeconds - Time.timeSinceLevelLoad;
        UIManager.MyInstance.time.text = remainingTime.ToString("0");

        if (remainingTime <= 0)
        {
            int lvl = SceneManager.GetActiveScene().buildIndex;
            LoadLevel(lvl);
        }

        // --- Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LoadLevel(int lvlIndex) //Event draus machen?
    {
        //Reset all UI elements
        UIManager.MyInstance.ResetUI();

        //Load Level
        SceneManager.LoadScene(lvlIndex);
    }

    // --- Pause
    public bool gameIsPaused = false;

    public void Pause()
    {
        UIManager.MyInstance.ingameUI.SetActive(false);
        UIManager.MyInstance.pauseUI.SetActive(true);

        Cursor.visible = true;

        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        print("RESUME");

        UIManager.MyInstance.ingameUI.SetActive(true);
        UIManager.MyInstance.pauseUI.SetActive(false);

        Cursor.visible = false;

        Time.timeScale = 1f;
        gameIsPaused = false;
    }
}
