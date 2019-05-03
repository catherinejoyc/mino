using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //INGAME
    public GameObject ingameUI;
    public Image TutorialScreen;
    public Slider VolumeIndicator;
    public Text keyFragments;
    public Text stonecount;
    public Text time;

    //ANIMATOR

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

    private void Start()
    {
        ResetUI();
        FadeFromBlack();
    }

    public void ResetUI()
    {
        keyFragments.text = "0/3";
        TutorialScreen.enabled = false;
    }

    //PAUSE
    public GameObject pauseUI;

    //DEATH
    public GameObject deathScreen;
    public void ShowDeathScreen()
    {
        //Death Screen
        GameManager.MyInstance.dead = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ingameUI.SetActive(false);
        pauseUI.SetActive(false);
        _fadeScreen.gameObject.SetActive(false);

        //show reason of death!
        deathScreen.SetActive(true);

        Time.timeScale = 0f;
        GameManager.MyInstance.gameIsPaused = true;
    }

    //SETTINGS
    public GameObject settingsScreen;
    public void ShowSettingsScreen()
    {


        Cursor.lockState = CursorLockMode.Confined;

        ingameUI.SetActive(false);
        pauseUI.SetActive(false);
        deathScreen.SetActive(false);
        settingsScreen.SetActive(true);
        _fadeScreen.gameObject.SetActive(false);

        Cursor.visible = true;
    }
    public void HideSettingsScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;

        ingameUI.SetActive(false);
        pauseUI.SetActive(true);
        deathScreen.SetActive(false);
        settingsScreen.SetActive(false);


        Cursor.visible = true;
    }

    //FADE
    public Animator _fadeScreen;
    public void FadeToBlack()
    {
        _fadeScreen.SetTrigger("fadeToBlack");
    }
    public void FadeFromBlack()
    {
        _fadeScreen.SetTrigger("fadeFromBlack");
    }
    
}
