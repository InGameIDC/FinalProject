using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
    public GameObject ShootingButton;
    public int numOfCurrentAreas;
    public int numOfMaxAreas;
    private bool isItMax = false;
    public GameObject bm; //battle manager

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(numOfCurrentAreas == numOfMaxAreas && !isItMax)
        {
            isItMax = true;
            ShootingButton.GetComponent<Button>().interactable = false;
            bm.GetComponent<BattleManager>().onPressShooting();
        }
        else if(numOfCurrentAreas != numOfMaxAreas && isItMax)
        {
            isItMax = false;
            ShootingButton.GetComponent<Button>().interactable = true;
        }
    }

    public void AddedArea()
    {
        numOfCurrentAreas++;
    }

    public void RemovedArea()
    {
        numOfCurrentAreas--;
    }
}
