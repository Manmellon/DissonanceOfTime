using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject mainScreen;
    public GameObject pauseScreen;

    [Header("Main Screen")]
    public Image cursorImage;
    public TextMeshProUGUI descriptionText;

    public bool isPaused;

    public Level[] levels;

    public static UI singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startScreen.SetActive(true);
        mainScreen.SetActive(false);
        pauseScreen.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !startScreen.activeSelf)
        {
            Pause();
        }
    }

    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;

            pauseScreen.SetActive(true);
            mainScreen.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;

            pauseScreen.SetActive(false);
            mainScreen.SetActive(true);
        }

    }

    public void Play()
    {
        startScreen.SetActive(false);
        mainScreen.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void RestartLevel()
    {
        //Show loading
        //Load level
        Pause();//Unpause
    }

    public void ReturnToMenu()
    {
        startScreen.SetActive(true); pauseScreen.SetActive(false); mainScreen.SetActive(false);
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
