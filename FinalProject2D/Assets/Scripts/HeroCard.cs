using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCard : MonoBehaviour
{
    public enum cardStatus { locked, opened, inUse};
    public enum upgradeStatus { ready, notReady};


    public cardStatus cardStat = cardStatus.opened;
    public upgradeStatus upgradeStat = upgradeStatus.notReady;
    public bool cardShow = false;
    public GameObject upgradeB;
    public GameObject useB;
    public GameObject infoB;
    public GameObject infoP;

    private void Start()
    {
        //get information from XML

        //get the object's buttons and panels
        /*
        upgradeB = GameObject.FindGameObjectWithTag("upgradeB");
        useB = GameObject.FindGameObjectWithTag("useB");
        infoB = GameObject.FindGameObjectWithTag("infoB");
        infoP = GameObject.FindGameObjectWithTag("infoP");
        */
    }

    public void onClick()
    {
        if (!cardShow)      //need to show buttons
        {
            if(cardStat == cardStatus.locked)
            {
                infoP.SetActive(true);
            }
            else if(cardStat == cardStatus.opened)
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);
                    upgradeB.transform.localPosition = new Vector3(0f, -51.035f, 0);
                    upgradeB.SetActive(true);

                }
                else
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);
                }
            }
            else        //inUse
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    upgradeB.transform.localPosition = new Vector3(0f, useB.transform.localPosition.y, 0f);
                    upgradeB.SetActive(true);
                }
                else
                {
                    infoB.SetActive(true);
                }
            }

            cardShow = true;
        }
        else                //need to close buttons
        {
            useB.SetActive(false);
            infoB.SetActive(false);
            infoP.SetActive(false);
            upgradeB.SetActive(false);
            cardShow = false;
        }
    }
}
