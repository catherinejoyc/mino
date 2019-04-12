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
}
