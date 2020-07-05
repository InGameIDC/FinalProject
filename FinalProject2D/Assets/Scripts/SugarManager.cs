using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class SugarManager : MonoBehaviour
{
    public float windMill = 0;     //num between 100 to -100.
    public int minSugars = 2;
    public int maxSugars = 4;
    public float timeToSpawn = 8f;
    [SerializeField] int currSugar = 0;
    private float timer = 0.0f;
    private bool checkField = false;
    private int[] sugarIdsList = { 0, 0, 0, 0, 0, 1, 1, 1, 2, 2 };

    private GameObject sugPreFab;
    public GameObject sugarPrefab0;
    public GameObject sugarPrefab1;
    public GameObject sugarPrefab2;

    // OrS for score bar
    public Action<float> OnScoreChange = delegate { };

    private void Update()
    {
        if (!checkField)
        {
            StartCoroutine(CheckSugarsOnField());
        }
    }

    public void score(int hillBalance)
    {
        currSugar--;
        if (hillBalance != 0)
        {
            windMill += hillBalance;
            OnScoreChange(windMill);
            //Debug.Log("Score: " + windMill);
        }
    }

    IEnumerator CheckSugarsOnField()
    {
        checkField = true;

        yield return new WaitForSeconds(0.7f);
        timer += Time.deltaTime;
        if (currSugar < minSugars)
        {
            sugPreFab = GetSugarPrefab();
            Vector3 pos = new Vector3(Random.Range(-1.92f, 1.92f), Random.Range(-3.35f, 3.35f), -0.5f);
            Instantiate(sugPreFab, pos, Quaternion.identity);
            currSugar++;
        }
        else if (currSugar < maxSugars)
        {
            //Debug.Log(timer);
            if(timer >= timeToSpawn)
            {
                sugPreFab = GetSugarPrefab();
                Instantiate(sugPreFab, new Vector3(Random.Range(-1.92f, 1.92f), Random.Range(-3.35f, 3.35f), -0.5f), Quaternion.identity);
                currSugar++;
                timer = 0.0f;
            }
        }

        checkField = false;
    }

    private GameObject GetSugarPrefab()
    {
        int sugarType = (int)Random.Range(0, sugarIdsList.Length);
        switch(sugarType){
            case 1:
                return sugarPrefab1;

            case 2:
                return sugarPrefab2;
  
            default:
                return sugarPrefab0;
        }
    }

    private Vector3 FindRandomLegalPos()
    {

    }

}
