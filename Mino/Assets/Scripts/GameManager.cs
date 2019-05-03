using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float timeOutInSeconds; //in sec
    float remainingTime;
    public bool dead = false;

    GameObject player;

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

        //get player
        try
        {       
            player = FindObjectOfType<PlayerController>().gameObject;
        }
        catch
        {
            Debug.Log("couldn't find playerController");
        }
            


        //remainingTime += Time.timeSinceLevelLoad;
        //print(remainingTime);
        Resume();
    }

    private void Update()
    {
        //Time Countdown
        remainingTime = timeOutInSeconds - Time.timeSinceLevelLoad;
        UIManager.MyInstance.time.text = remainingTime.ToString("0");

        if (remainingTime <= 0)
        { 
            TimeRunOut();
        }

        // --- Pause
        if (Input.GetKeyDown(KeyCode.Escape) &&!dead)
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

        //Reset time
        remainingTime = timeOutInSeconds;

        //Load Level
        if (lvlIndex == 0) //Main Menu
        {
            Debug.Log("Main Menu");
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Debug.Log("Resume");
            Resume();
        }

        SceneManager.LoadScene(lvlIndex);
    }

    public void RestartLevel()
    {
        int lvl = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(lvl);
    }

    // --- Menu
    public bool gameIsPaused = false;

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;

        UIManager.MyInstance.ingameUI.SetActive(false);
        UIManager.MyInstance.pauseUI.SetActive(true);
        UIManager.MyInstance.deathScreen.SetActive(false);
        UIManager.MyInstance.settingsScreen.SetActive(false);
        UIManager.MyInstance._fadeScreen.gameObject.SetActive(false);

        Cursor.visible = true;

        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;

        UIManager.MyInstance.ingameUI.SetActive(true);
        UIManager.MyInstance.pauseUI.SetActive(false);
        UIManager.MyInstance.deathScreen.SetActive(false);
        UIManager.MyInstance.settingsScreen.SetActive(false);

        Cursor.visible = false;

        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //SETTINGS
    public AK.Wwise.RTPC volumeRTPC;
    public void Settings()
    {
        UIManager.MyInstance.ShowSettingsScreen();

        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void SetVolume(float volume)
    {
        //change volume in wwise
        volumeRTPC.SetGlobalValue(volume);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().cameraSensitivity = sensitivity;          
        }
    }

    //Timeout
    void TimeRunOut()
    {
        UIManager.MyInstance.ShowDeathScreen();
    }
}
