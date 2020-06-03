using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Author: OrS
/// This class is responsible for the level buttons
/// it is attached to every level button
/// </summary>
public class LevelSelection : MonoBehaviour
{
    //-------Outside scripts and classes-------//
    GameObject gs;

    [SerializeField] private bool unlocked; // Default value is false

    //-------Outside GameObjects-------//
    public Image lockImage;

    private bool played = false;

    //public Action<int> OnPressingLevel = delegate { };



    private void Start()
    {
        //initiating the gameobjects for future use
        gs = GameObject.FindGameObjectWithTag("GameStatus");
        
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
        }
    }

}
