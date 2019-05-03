using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

    // Main Menu
    public int startLevelIndex;

    //Animator
    public Animator startPressed_Animator;
    public Animator quitPressed_Animator;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void MainMenu_StartGame()
    {
        //play fade in StartPressed Image
        startPressed_Animator.SetTrigger("startPressed");

        //load scene after animation
        Invoke("StartGame", 1.5f);

    }
    void StartGame()
    {
        SceneManager.LoadScene(startLevelIndex);
    }

    public void MainMenu_Quit()
    {
        //play fade in QuitPressed Image
        quitPressed_Animator.SetTrigger("quitPressed");

        //quit game after animation
        Invoke("Quit", 1.5f);
    }
    void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //SETTINGS
    public AK.Wwise.RTPC volumeRTPC;

    //SETTINGS.UI
    public GameObject settingsScreen;

    public void Settings()
    {
        Cursor.lockState = CursorLockMode.Confined;

        settingsScreen.SetActive(true);

        Cursor.visible = true;
    }
    public void SetVolume(float volume)
    {
        //change volume in wwise
        volumeRTPC.SetGlobalValue(volume);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        //SZENENÜBERGREIFENDE VARIABLEN ERSTELLEN
        //if (player != null)
        //{
        //    player.GetComponent<PlayerController>().cameraSensitivity = sensitivity;
        //}
    }
}
