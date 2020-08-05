using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: OrS
/// This class is responsible for the level buttons
/// it is attached to every level button
/// </summary>
public class LevelSelection : MonoBehaviour
{
    //-------Outside scripts and classes-------//
    GameObject gs;

    private bool unlocked = false; // Default value is false

    //-------Outside GameObjects-------//
    public Image lockImage;

    private bool played = false;
    private int starsEarned = 0;

    private Color32 originalColor;
    public int levelId;

    public GameObject[] starsOfLevel;

    //public Action<int> OnPressingLevel = delegate { };



    private void Start()
    {
        //initiating the gameobjects for future use
        gs = GameObject.FindGameObjectWithTag("GameStatus");

        originalColor = GetComponent<Image>().color;

        if(levelId != 1)
        {
            if (gs.GetComponent<GameStatus>().levelsPlayed[levelId - 2] == 1 && gs.GetComponent<GameStatus>().starsToUnlock[levelId - 1] <= gs.GetComponent<GameStatus>().xpLevel)
            {
                unlocked = true;
            }
        }
        else
        {
            unlocked = true;
        }
            

        if(gs.GetComponent<GameStatus>().levelsPlayed[levelId - 1] == 1)
        {
            played = true;
            starsEarned = gs.GetComponent<GameStatus>().starsInLevels[levelId - 1];
        }
        

        //update the display of the level button
        UpdateLevelImage();
    }

    /// <summary>
    /// Author: OrS
    /// This Function updates the display of the level button
    /// </summary>
    private void UpdateLevelImage()
    {
        if (!unlocked)
        {
            lockImage.gameObject.SetActive(true);
            gameObject.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if(played)
            {
                gameObject.GetComponent<Image>().color = new Color32(221, 163, 32, 255);
                
                if (starsEarned == 1)
                {
                    starsOfLevel[0].SetActive(false);
                }
                else if (starsEarned == 2)
                {
                    starsOfLevel[0].SetActive(false);
                    starsOfLevel[1].SetActive(false);
                }
                else if(starsEarned == 3)
                {
                    starsOfLevel[0].SetActive(false);
                    starsOfLevel[1].SetActive(false);
                    starsOfLevel[2].SetActive(false);
                }
            }
            lockImage.gameObject.SetActive(false);
        }
        
    }

    /// <summary>
    /// Author:OrS
    /// upon selecting a level, if it is unlocked - it loads the heros to choose scene and saves the level chosen in the game status
    /// </summary>
    /// <param name="levelidx"></param>
    public void PressSelection(int levelidx)
    {
        if (unlocked)
        {
            gs.GetComponent<GameStatus>().lastLevelCosen = levelidx;
            gs.GetComponent<GameStatus>().isToLevel = 1;
            //gameObject.GetComponent<Image>().color = Color.green;
            //OnPressingLevel(levelidx);
            SceneManager.LoadScene("ChooseHeroes");

        }
    }

    public void unPress()
    {
        //gameObject.GetComponent<Image>().color = originalColor;
    }

}
