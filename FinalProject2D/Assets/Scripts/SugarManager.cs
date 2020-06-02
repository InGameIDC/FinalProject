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
    public int currSugar = 0;
    private float timer = 0.0f;

    public GameObject sugarPrefab;

    // OrS for score bar
    public Action<float> OnScoreChange = delegate { };

    public void score(int hillBalance)
    {
        if (hillBalance != 0)
        {
            windMill += hillBalance;
            OnScoreChange(windMill);
            //Debug.Log("Score: " + windMill);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(currSugar < minSugars)
        {
            Instantiate(sugarPrefab, new Vector3(Random.Range(-1.92f, 1.92f), Random.Range(-3.35f, 3.35f), -0.5f), Quaternion.identity);
            currSugar++;
        }
        else if(currSugar < maxSugars)
        {
            Debug.Log(timer);
            if(timer >= timeToSpawn)
            {
                Instantiate(sugarPrefab, new Vector3(Random.Range(-1.92f, 1.92f), Random.Range(-3.35f, 3.35f),-0.5f), Quaternion.identity);
                currSugar++;
                timer = 0.0f;
            }
        } 
    }

}
