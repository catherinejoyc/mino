using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

    // Main Menu
    public int startLevelIndex;
    public Button startBtn;

    //Animator
    public Animator startPressed_Animator;
    public Animator quitPressed_Animator;

    private void Awake()
    {
        Time.timeScale = 1;

        SetVolume(optionsVariables.volume);
        volume.value = optionsVariables.volume;
        SetMouseSensitivity(optionsVariables.mouseSensitivity);
        mouseSensitivity.value = optionsVariables.mouseSensitivity;
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
    public OptionsMenuVariables optionsVariables;

    public GameObject settingsScreen;
    public Slider volume;
    public Slider mouseSensitivity;

    public void ShowSettingsScreen(Button btn)
    {
        Cursor.lockState = CursorLockMode.Confined;


        settingsScreen.SetActive(true);
        creditsScreen.SetActive(false);
        btn.Select();

        Cursor.visible = true;
    }
    public void HideSettingsScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;

        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        startBtn.Select();

        Cursor.visible = true;
    }

    //public void Settings()
    //{
    //    Cursor.lockState = CursorLockMode.Confined;

    //    settingsScreen.SetActive(true);

    //    Cursor.visible = true;
    //}
    public void SetVolume(float volume)
    {
        //change volume in wwise
        optionsVariables.volume = volume;
        volumeRTPC.SetGlobalValue(volume);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        //SZENENÜBERGREIFENDE VARIABLEN ERSTELLEN
        //if (player != null)
        //{
        //    player.GetComponent<PlayerController>().cameraSensitivity = sensitivity;
        //}
        optionsVariables.mouseSensitivity = sensitivity;
    }

    //CREDITS
    public GameObject creditsScreen;
    public void ShowCreditsScreen(Button btn)
    {
        Cursor.lockState = CursorLockMode.Confined;

        settingsScreen.SetActive(false);
        creditsScreen.SetActive(true);

        btn.Select();

        Cursor.visible = true;
    }
    public void HideCreditsScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;

        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        startBtn.Select();

        Cursor.visible = true;
    }

    //CONTROLS
    public GameObject controlsScreen;
    public void ShowControlsScreen(Button btn)
    {
        Cursor.lockState = CursorLockMode.Confined;

        controlsScreen.SetActive(true);
        btn.Select();

        Cursor.visible = true;
    }
    public void HideControlsScreen()
    {
        controlsScreen.SetActive(false);

        startBtn.Select();
    }

}
