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
    /// <summary>
    /// Author:OrS
    /// upon clicking the collection button this method loads the heros to choose scene
    /// </summary>
    public void goToCollection()
    {
        SceneManager.LoadScene("ChooseHeroes");
    }
}
