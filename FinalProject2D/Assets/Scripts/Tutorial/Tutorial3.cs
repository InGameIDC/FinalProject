using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3 : MonoBehaviour
{
    [SerializeField] GameObject _Part1;
    [SerializeField] GameObject _Part2;
    [SerializeField] GameObject _Part3;
    [SerializeField] GameObject _UIPart1;
    [SerializeField] GameObject _UIPart2;
    [SerializeField] GameObject _UIPart3;
    [SerializeField] GameObject _Blocker;
    
    bool isFirstTimeLoatPart3 = true;

    // Start is called before the first frame update
    void Start()
    {
        // case that the player already finished the tutorial
        if (PlayerPrefs.GetInt("FinishedTutorial", 0) == 1)
        {
            if (PlayerPrefs.GetInt("FinishedMenuTutorial", 0) == 1)
                SceneManager.LoadScene("HomeMenu");
            else
                SceneManager.LoadScene("TutorialHomeMenu");
        }

        GameObject.Find("HeroIcon 2").GetComponent<HeroIcon>().ActiveOnClick += loadPart2;
        StartCoroutine(pauseAfterDelay(0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadPart2(GameObject unit)
    {
        GameObject.Find("HeroIcon 2").GetComponent<HeroIcon>().ActiveOnClick -= loadPart2;
        // disactive part 1
        _Part1.SetActive(false);
        _UIPart1.SetActive(false);
        // active part 1
        _Part2.SetActive(true);
        _UIPart2.SetActive(true);

        Time.timeScale = 1;

    }

    private IEnumerator pauseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
    }

    public void loadPart3()
    {
        _Part2.SetActive(false);
        _UIPart2.SetActive(false);
        _Blocker.SetActive(false);
        _Part3.SetActive(true);
        _UIPart3.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "HeroUnit")
        {
            if (isFirstTimeLoatPart3)
            {
                loadPart3();
                isFirstTimeLoatPart3 = false;
            }
        }
    }
}
