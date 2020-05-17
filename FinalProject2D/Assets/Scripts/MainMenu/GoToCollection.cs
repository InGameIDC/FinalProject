using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: OrS
/// this class moves the player to the heroes to choose scene in a collection mode
/// TODO: collect all the buttons scripts into one
/// </summary>
public class GoToCollection : MonoBehaviour
{
    //-------Outside scripts and classes-------//
    public GameObject gs; 

    /// <summary>
    /// Author:OrS
    /// upon clicking the collection button this method loads the heros to choose scene
    /// </summary>
    public void goToCollection()
    {
        gs.GetComponent<GameStatus>().isToLevel = 0;
        SceneManager.LoadScene("ChooseHeroes");
    }
}
