using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadScore : MonoBehaviour
{

    public TMP_Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            timerText.text = "Best Score: " + Mathf.Floor(PlayerPrefs.GetFloat("Score"));
        }
        else
        {
            timerText.text = "";
        }
    }

}
