using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject arrow1;
    [SerializeField] GameObject arrow2;
    [SerializeField] GameObject arrow3;
    [SerializeField] GameObject _GoButton;
    [SerializeField] GameObject _BackMenu;

    bool isFirstTime2 = true;
    bool isFirstTime3 = true;
    bool isFirstTime4 = true;

    private void Start()
    {
        if(PlayerPrefs.GetInt("FinishedMenuTutorial", 0) == 1)
            SceneManager.LoadScene("ChooseHeroes");
    }

    public void ActivePart2()
    {
        if (isFirstTime2)
        {
            arrow1.SetActive(false);
            arrow2.SetActive(true);
            isFirstTime2 = false;
        }
    }

    public void ActivePart3()
    {
        if (isFirstTime3)
        {
            arrow2.SetActive(false);
            arrow3.SetActive(true);
            isFirstTime3 = false;
        }
    }

    public void ActivePart4()
    {
        if (isFirstTime4)
        {
            arrow3.SetActive(false);
            _GoButton.SetActive(true);
            _BackMenu.SetActive(true);
            isFirstTime4 = false;
            PlayerPrefs.SetInt("FinishedMenuTutorial", 1);
            SceneManager.LoadScene("ChooseHeroes");
        }
    }

}
