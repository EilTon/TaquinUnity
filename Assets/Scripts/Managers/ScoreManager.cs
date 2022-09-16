using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TeasingGame;

public class ScoreManager : MonoBehaviour
{
    #region Declaration 

    public static ScoreManager instance; // singleton
    public float scoreMax = 600; // The Score where it's start
    public float timeMax = 180; // The max time the player have to finish the puzzle
    public TMP_Text timerText; // ref UI to display the timer text 
    public TeasingGameHomeSceneController transition; // Script to return home
    private float currentScore = 0; // the current score
    private float timeRemain; // the time remain
    private float step; // step to decrease the current score
    private bool isTimerOn = true; // boolean that check is the time is On or Off

    #endregion

    #region Unity Functions 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeRemain = timeMax;
        currentScore = scoreMax;
        step = scoreMax / timeMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemain > 0 && isTimerOn)
        {
            timeRemain -= Time.deltaTime;
            currentScore -= step * Time.deltaTime;
            UpdateTimer(timeRemain);
        }
        else
        {
            timeRemain = 0;
            LoadSceneHome();
        }

    }

    #endregion

    #region Main Functions 

    // update the timer with a specific format
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void SetIsTimerOn(bool isTimerOn)
    {
        this.isTimerOn = isTimerOn;
    }

    // Check if a score is save or not and load scene Home
    public void LoadSceneHome()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            if (currentScore > PlayerPrefs.GetFloat("Score"))
            {
                PlayerPrefs.SetFloat("Score", currentScore);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("Score", currentScore);
        }

        transition.GoToGameScene();

    }

    #endregion
}
