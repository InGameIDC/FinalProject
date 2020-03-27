using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetsBank : MonoBehaviour
{
    Action<GameObject> OnTargetInFieldOfView = delegate { };

    private List<GameObject> _targetsToAttackBank;
    private bool _isScanning;


    // Start is called before the first frame update
    void Start()
    {
        
    }



}
