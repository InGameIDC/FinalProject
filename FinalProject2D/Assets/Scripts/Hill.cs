using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hill : MonoBehaviour
{
    public float hillRadius;
    //public float windMill;     //num between 100 to -100 .
    public int scoreToAdd;
    public int hillBalance;
    public float pointsCalcInterval;
    public float secondsToAppear = 15;
    private float timer = 0.0f;
    public GameObject pointsAddedPrefab;
    public int[] sugarScores = { 5, 10, 20 };
    public int sugarId;


    public GameObject sugarManagerObject;
    private SugarManager sugarManager;


    private void Awake()
    {
        sugarManagerObject = GameObject.FindGameObjectWithTag("SugarManager");
        sugarManager = sugarManagerObject.GetComponent<SugarManager>();
        hillRadius = GetComponent<CircleCollider2D>().radius;
        pointsCalcInterval = 0.3f;
        //Test.DrawCircle(gameObject, hillRadius, 0.05f, Color.cyan);
        //InvokeRepeating("score", 1f, pointsCalcInterval);
    }

    /*
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= secondsToAppear)
        {
            sm.GetComponent<SugarManager>().currSugar--;
            Destroy(gameObject);
        }
    }
    */

    /// <summary>
    /// author : dor peretz
    /// descrition : sets a red circle around the hill object
    /// </summary>
    private void hillRadiusDebugger()
    {
        //Test.DrawCircle(gameObject, hillRadius, 1f);
    }

    /// <summary>
    /// author : dor peretz
    /// descrition : adds a point to the num of enemys in the radius
    /// and call for recalculating the scroe function.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<CircleCollider2D>().isTrigger = false; // after one gets in turn off so no other player can get points

        

        //Debug.Log(collision.transform.parent.name);

        GameObject unit = collision.gameObject;
        if(unit.tag == "EnemyUnit")
        {
            //pointsAddedPrefab.GetComponent<TMPro.TextMeshProUGUI>().text = "-" + sugarScores[sugarId];
            sugarManager.score(-sugarScores[sugarId]);  //to change according to cube type
            Destroy(gameObject);

        }

        if (unit.tag == "HeroUnit")
        {
            //pointsAddedPrefab.SetActive(true);
            //pointsAddedPrefab.GetComponent<TMPro.TextMeshProUGUI>().text = "" + sugarScores[sugarId];
            Instantiate(pointsAddedPrefab, new Vector3(transform.position.x , transform.position.y, 0 ), Quaternion.identity);
            //Destroy(gameObject);
            sugarManager.Animate(gameObject);
            sugarManager.score(sugarScores[sugarId]);  //to change according to cube type
        }

        
    }


    /*
    /// <summary>
    /// author : dor peretz
    /// descrition : deducts a point to the num of enemys in the radius
    /// and call for recalculating the scroe function.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        

        GameObject unit = collision.gameObject;
        if (unit.tag == "EnemyUnit")
        {
            numOfEnterdEnemys--;
        }

        if (unit.tag == "HeroUnit")
        {
            numOfEnterdHeros--;
        }

        hillUnitsBalance();
    }

    /// <summary>
    /// author: dor peretz 
    /// </summary>
    private void hillUnitsBalance()
    {
        hillBalance = numOfEnterdHeros - numOfEnterdEnemys;
    }

    
    /// <summary>
    /// author: dor peretz 
    /// </summary>
    private void score()
    {
        sm.GetComponent<SugarManager>().score(hillBalance);
        
    }
    */
}
