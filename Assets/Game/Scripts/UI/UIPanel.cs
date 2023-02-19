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
    public GameObject settingsPanel;
    public GameObject helpPanel;

    [Header("Main Panel")]
    public Button playButton;
    public Button continueButton;
    public Button restartLevelButton;
    public Button chooseLevelButton;
    public Button settingsButton;
    public Button helpButton;
    public Button mainMenuButton;
    public Button exitButton;

    [Header("Levels Panel")]
    public Image levelThumbnailImage;
    public TextMeshProUGUI levelNameText;
    public Button levelPanelPlayButton;
    public Transform levelButtonsParent;
    public ToggleGroup levelToggleGroup;
    public UILevelToggle levelTogglePrefab;

    [Header("Settings Panel")]
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;

    public Dropdown qualityDropdown;

    //[Header("Help Panel")]
    //public TextMeshProUGUI helpText;

    void Start()
    {
        backButton.onClick.AddListener(() => { levelsPanel.SetActive(false); settingsPanel.SetActive(false); helpPanel.SetActive(false);
                                                mainPanel.SetActive(true); backButton.gameObject.SetActive(false); });

        //Start screen
        playButton.onClick.AddListener(() => { UI.singleton.Play(); } );

        continueButton.onClick.AddListener(() => { UI.singleton.Pause(); });

        restartLevelButton.onClick.AddListener(() => { UI.singleton.LoadLevel(UI.singleton.curLevel); });

        chooseLevelButton.onClick.AddListener(() => { mainPanel.SetActive(false); levelsPanel.SetActive(true); backButton.gameObject.SetActive(true); });

        settingsButton.onClick.AddListener(() => { mainPanel.SetActive(false); settingsPanel.SetActive(true); backButton.gameObject.SetActive(true); });

        helpButton.onClick.AddListener(() => { mainPanel.SetActive(false); helpPanel.SetActive(true); backButton.gameObject.SetActive(true); });

        mainMenuButton.onClick.AddListener(() => { UI.singleton.ReturnToMenu(); });

        exitButton.onClick.AddListener(() => { UI.singleton.QuitGame(); });

        //Levels Panel
        levelPanelPlayButton.onClick.AddListener(() => { UI.singleton.LoadLevel(levelToggleGroup.GetFirstActiveToggle().transform.GetSiblingIndex()); });
        ChooseLevel(0, true);
        foreach (var level in UI.singleton.levels)
        {
            UILevelToggle levelToggle = Instantiate(levelTogglePrefab, levelButtonsParent);
            levelToggle.numberText.text = levelToggle.transform.GetSiblingIndex().ToString();
            levelToggle.toggle.group = levelToggleGroup;
            levelToggle.toggle.onValueChanged.AddListener((value) => { ChooseLevel(levelToggle.transform.GetSiblingIndex(), value); });
        }

        //Settings Panel
    }

    public void ChooseLevel(int index, bool enable)
    {
        if (!enable) return;

        levelThumbnailImage.sprite = UI.singleton.levels[index].thumbnailSprite;
        levelNameText.text = UI.singleton.levels[index].levelName;
    }

}
