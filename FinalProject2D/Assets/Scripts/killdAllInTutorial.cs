using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class killdAllInTutorial : MonoBehaviour
{
    public GameObject gas;
    private GameStatus gs;
    public GameObject carrot1;
    public GameObject carrot2;
    public GameObject carrot3;
    public GameObject endTutorial2panel;

    private int toKill = 3;

    // Start is called before the first frame update
    void Start()
    {
        
        gs = gas.GetComponent<GameStatus>();
        
        //Debug.Log(hill.name);
        carrot1.GetComponentInChildren<Health>().OnDeath += onDeath;
        carrot2.GetComponentInChildren<Health>().OnDeath += onDeath;
        carrot3.GetComponentInChildren<Health>().OnDeath += onDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if(toKill == 0)
        {
            gs.tutorialPlayed = 1;
            endTutorial2panel.SetActive(true);
            StartCoroutine(nextScence());
        }
    }

    public IEnumerator nextScence()
    {
        yield return new WaitForSeconds(2f);
        //GetComponent<ResetGameStatus>().ResetGameStatusData(false);
        
        SceneManager.LoadScene("HomeMenu");
    }
    void onDeath(GameObject go)
    {
        toKill--;
    }
}
