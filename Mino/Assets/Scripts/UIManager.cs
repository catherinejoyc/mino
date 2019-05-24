using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //INGAME
    public GameObject ingameUI;
    public Image TutorialScreen;
    public Slider VolumeIndicator;
    //options
    public Slider mouseSensitivity;
    public Slider volume;
    //public Text keyFragments;
    public UINumberSpriteSheetScript keyNumberSpriteSheet;
    public UINumberSpriteSheetScript stoneNumberSpriteSheet;
    public Animator keyEffect;
    public Image bleedingScreen;
    //public Text stonecount;
    //public Text time;

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
        //keyFragments.text = "0/3";
        keyNumberSpriteSheet.ChangeNumberSprite(0);
        TutorialScreen.enabled = false;
        volume.value = GameManager.MyInstance.optionsVariables.volume;
        mouseSensitivity.value = GameManager.MyInstance.optionsVariables.mouseSensitivity;
    }

    //play key effect
    public void PlayKeyfragmentEffect01()
    {
        keyEffect.SetTrigger("keyfragmentEffect01");
    }
    public void PlayKeyfragmentEffect02()
    {
        keyEffect.SetTrigger("keyfragmentEffect02");
    }

    //ADD KEY TO COUNT
    public void AddKey(int _keyCount)
    {
        keyNumberSpriteSheet.ChangeNumberSprite(_keyCount);
    }

    //ADD STONE TO COUNT
    public void AddStone(int _stoneCount)
    {
        stoneNumberSpriteSheet.ChangeNumberSprite(_stoneCount);
    }

    //PAUSE
    public GameObject pauseUI;
    public Button pauseFirstButton;

    //DEATH
    public GameObject deathScreen;
    public Button deathScreenRestartBtn;
    public void ShowDeathScreen()
    {
        //Death Screen
        GameManager.MyInstance.dead = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SelectBtnFirst(deathScreenRestartBtn);

        ingameUI.SetActive(false);
        pauseUI.SetActive(false);
        _fadeScreen.gameObject.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
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
        controlsScreen.SetActive(false);

        Cursor.visible = true;
    }
    //public void HideSettingsScreen()
    //{
    //    Cursor.lockState = CursorLockMode.Confined;

    //    ingameUI.SetActive(false);
    //    pauseUI.SetActive(true);
    //    deathScreen.SetActive(false);
    //    settingsScreen.SetActive(false);


    //    Cursor.visible = true;
    //}

    //CONTROLS
    public GameObject controlsScreen;
    public void ShowControlsScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;

        ingameUI.SetActive(false);
        pauseUI.SetActive(false);
        deathScreen.SetActive(false);
        settingsScreen.SetActive(false);
        _fadeScreen.gameObject.SetActive(false);
        controlsScreen.SetActive(true);

        Cursor.visible = true;
    }

    //BACK
    public void SelectBtnFirst(Button backBtn)
    {
        backBtn.Select();
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


    IEnumerator FadeBleedingScreen(bool fadeAway, float fadeTime)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                bleedingScreen.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= fadeTime; i += Time.deltaTime)
            {
                // set color with i as alpha
                bleedingScreen.color = new Color(1, 1, 1, i);
                yield return null;
            }

            // loop over 1 second backwards
            for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                bleedingScreen.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    public void StartBleedingScreen(float fadeTime)
    {
        StartCoroutine(FadeBleedingScreen(false, fadeTime));
        print("bleed");
    }
    //public void StopBleedingScreen(float fadeTime)
    //{
    //    StartCoroutine(FadeBleedingScreen(true, fadeTime));
    //}
    public void AttackBleedingScreen(float fadeTime)
    {
        StartCoroutine(FadeBleedingScreen(true, fadeTime));
        print("stop bleed");

        //StartCoroutine(FadeBleedingScreen(false, 0.5f));
        //StartCoroutine(FadeBleedingScreen(true, 0.1f));
    }
}
