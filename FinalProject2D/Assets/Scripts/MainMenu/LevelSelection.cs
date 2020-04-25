using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelSelection : MonoBehaviour
{

    [SerializeField] private bool unlocked; // Default value is false
    public Image lockImage;
    GameObject gs;

    private void Start()
    {
        UpdateLevelImage();

        gs = GameObject.FindGameObjectWithTag("GameStatus");
    }

    private void UpdateLevelImage()
    {
        if (!unlocked)
        {
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            lockImage.gameObject.SetActive(false);
        }
    }

    public void PressSelection(string levelName)
    {
        if (unlocked)
        {
            gs.GetComponent<GameStatus>().lastLevelCosen = levelName;
            SceneManager.LoadScene("ChooseHeroes");
        }
    }

}
