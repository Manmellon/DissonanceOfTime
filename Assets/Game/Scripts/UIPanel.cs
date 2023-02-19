using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPanel : MonoBehaviour
{
    public Button backButton;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject levelsPanel;
    public GameObject optionsPanel;
    public GameObject helpPanel;

    [Header("Main Panel")]
    public Button playButton;
    public Button continueButton;
    public Button restartLevelButton;
    public Button chooseLevelButton;
    public Button optionsButton;
    public Button helpButton;
    public Button mainMenuButton;
    public Button exitButton;

    [Header("Levels Panel")]
    public Transform levelButtonsParent;

    [Header("Settings Panel")]
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;

    public Dropdown qualityDropdown;

    //[Header("Help Panel")]
    //public TextMeshProUGUI helpText;

    void Start()
    {
        //Start screen
        playButton.onClick.AddListener(() => { UI.singleton.Play(); } );

        continueButton.onClick.AddListener(() => { UI.singleton.Pause(); });

        mainMenuButton.onClick.AddListener(() => { UI.singleton.ReturnToMenu(); });

        exitButton.onClick.AddListener(() => { UI.singleton.QuitGame(); });
    }

}
