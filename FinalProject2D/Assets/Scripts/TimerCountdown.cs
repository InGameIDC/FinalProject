using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Author:OrS
/// This class is for the timer countdown
/// it counts the time backwards and in case that the timer is up, it is finishes the battle
/// </summary>
public class TimerCountdown : MonoBehaviour
{
    public GameObject textDisplay;                          //the timer text display
    public int secondsLeft = 135;                           //seconds left to the battle
    public bool takingAway = false;                         //need to reduce time from timer
    public GameObject endPanel;

    private void Start()
    {
        textDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "2:15";
    }

    private void Update()
    {
        if(takingAway == false && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        }
        else if (secondsLeft == 30) // used to play the RUSH sound
        {
            // Play rush sound
            SoundManager.Instance.PlaySound(Sound.RushTime);
        } 
        else if(secondsLeft == 0)
        {
            endPanel.SetActive(true);
        }
    }

    /// <summary>
    /// reduce 1 second every second and display it in the timer
    /// Author: OrS
    /// </summary>
    /// <returns></returns>
    IEnumerator TimerTake()
    {
        string zero = "";
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        //Debug.Log(secondsLeft);

        if(secondsLeft >= 120)
        {
            textDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "2:" + CheckSeconds(secondsLeft - 120);
        }
        else if(secondsLeft < 120 && secondsLeft >= 60)
        {
            textDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "1:" + CheckSeconds(secondsLeft - 60);
        }
        else if(secondsLeft < 60)
        {
            textDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "0:" + CheckSeconds(secondsLeft);
        }
        
        takingAway = false;
    }

    /// <summary>
    /// check if the seconds left (until round minute) are less then 10, and if so fix the string from 0 to 00 (add zero in order to look like a timer),
    /// returns the fixed string.
    /// Author: OrS
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private string CheckSeconds(int seconds)
    {
        if(seconds < 10)
        {
            return "0" + seconds;
        }
        else
        {
            return "" + seconds;
        }
    }
}
