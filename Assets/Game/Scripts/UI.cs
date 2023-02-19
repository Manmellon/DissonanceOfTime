using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject mainScreen;
    public GameObject pauseScreen;

    [Header("Start Screen")]

    [Header("Main Screen")]
    public Image cursorImage;
    public TextMeshProUGUI descriptionText;

    [Header("Pause Screen")]

    public static UI singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }
}
