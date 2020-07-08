using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingModeButton : MonoBehaviour
{
    public GameObject shootButtonBack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InShootingMode(bool inShooting)
    {
        if (inShooting)
        {
            shootButtonBack.SetActive(true);
        }
        else
        {
            shootButtonBack.SetActive(false);
        }
    }
}
