using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToCollection : MonoBehaviour
{
    // Start is called before the first frame update
    public void goToCollection()
    {
        SceneManager.LoadScene("ChooseHeroes");
    }
}
