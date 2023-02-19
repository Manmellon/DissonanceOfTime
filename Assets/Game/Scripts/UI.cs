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

    [Header("Start Screen")]
    public Button startButton;
    public Button chooseLevelButton_start;
    public Button optionsButton_start;
    public Button helpButton_start;
    public Button exitButton_start;

    [Header("Main Screen")]
    public Image cursorImage;
    public TextMeshProUGUI descriptionText;

    [Header("Pause Screen")]
    public Button continueButton;
    public Button restartLevelButton;
    public Button chooseLevelButton_pause;
    public Button optionsButton_pause;
    public Button helpButton_pause;
    public Button mainMenuButton;
    public Button exitButton_pause;

    public bool isPaused;

    public static UI singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Start screen
        startButton.onClick.AddListener(() => { startScreen.SetActive(false); mainScreen.SetActive(true); Pause(); });

        //Main screen

        //Pause screen
        continueButton.onClick.AddListener(() => { pauseScreen.SetActive(false); mainScreen.SetActive(true); Cursor.lockState = CursorLockMode.Locked; });
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
}
