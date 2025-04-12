using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public TMP_Text title;
    public Button play;
    public Button settings;
    public Button quit;
    bool isInSettings = false;


    void SetupButtonPosition(Button btn, Vector2 anchoredPos)
    {
        RectTransform rt = btn.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
    }

    void Start()
    {

        title.text = "Island Escape";
        title.alignment = TMPro.TextAlignmentOptions.Center;
        RectTransform rt = title.GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);

        rt.anchoredPosition = new Vector2(0, -100f);


        SetupButtonPosition(play, new Vector2(0f, 50f));     
        SetupButtonPosition(settings, new Vector2(0f, -20f));  
        SetupButtonPosition(quit, new Vector2(0f, -90f));      


        play.onClick.AddListener(startGame);
        settings.onClick.AddListener(options);
        quit.onClick.AddListener(quitGame);


        play.onClick.AddListener(startGame);
        settings.onClick.AddListener(options);
        quit.onClick.AddListener(quitGame);


    }

    void startGame()
    {

        Debug.Log("Play button pressed!");


    }



    void options()
    {
        isInSettings = !isInSettings;
        Debug.Log("Settings button pressed!");

    }

    void quitGame()
    {
        Debug.Log("Quit button pressed!");

    }

    // Update is called once per frame
    void Update()
    {
       



    }
}
