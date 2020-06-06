using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject[] levels;

    // Start is called before the first frame update
    void Start()
    {
        levels = GameObject.FindGameObjectsWithTag("levelButton");
    }

    // Update is called once per frame
    public void onLevelClick(int level)
    {
        for(int i = 0; i < levels.Length; i++)
        {
            if(levels[i].GetComponent<LevelSelection>().levelId != level)
            {
                levels[i].GetComponent<LevelSelection>().unPress();
            }
        }
    }

}
