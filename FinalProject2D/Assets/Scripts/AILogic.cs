using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    AI1 _AI1;
    AI2 _AI2;

    // Start is called before the first frame update
    void Start()
    {
        _AI1 = GetComponent<AI1>();
        _AI2 = GetComponent<AI2>();
        _AI1.OnNoTarget += onNoTarget;
        _AI2.OnNoHill += onNoHill;
    }

    private void onNoHill()
    {
        _AI1.enabled = true;
        _AI2.enabled = false;
    }

    private void onNoTarget()
    {
        _AI1.enabled = false;
        _AI2.enabled = true;
    }


}
