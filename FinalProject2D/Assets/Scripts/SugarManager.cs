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
            Vector3 pos = FindRandomLegalPos();
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
        float cleanRadius = 0.1f; // the radius around the suagar hill that shouldnt be with obsticales
        Vector3 pos = Vector3.zero;
        Collider2D[] damageHitAreasInSphere;
        LayerMask maskObstacles = LayerMask.GetMask("Obstacle");

        for (int i = 0; i < 50; i++) // generate until max 100 iteration, positions, until finds a one that doesnot overlap with obsticale 
        {
            pos = new Vector3(Random.Range(-1.92f, 1.92f), Random.Range(-3.35f, 3.35f), -0.5f);
            damageHitAreasInSphere = Physics2D.OverlapCircleAll(pos, cleanRadius, maskObstacles);
            if (damageHitAreasInSphere.Length == 0)
                return pos;

            // FOR DEBUG USES, PLEASE DO NOT REMOVE:
            //testFindRandom(damageHitAreasInSphere, pos);
        }

        return pos;
    }

    /// <summary>
    /// For Debug Tests
    /// </summary>
    private void testFindRandom(Collider2D[] damageHitAreasInSphere, Vector3 pos)
    {
        string obstaclesNames = "";
        for (int j = 0; j < damageHitAreasInSphere.Length; j++)
        {
            obstaclesNames += damageHitAreasInSphere[j].name + ",   ";
        }
        Debug.Log("Nope: " + damageHitAreasInSphere.Length + "  |   " + pos + "Name: " + obstaclesNames);
        StartCoroutine(Test.MarkCircleAtPos(pos, 20f));
    }
}
