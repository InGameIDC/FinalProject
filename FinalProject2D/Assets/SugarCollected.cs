using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarCollected : MonoBehaviour
{
    public int sugarCollected = 0;
    public GameObject sugarCollectedText;
    private Health healthObject;


    // Start is called before the first frame update
    void Start()
    {
        healthObject = transform.parent.gameObject.GetComponentInChildren<Health>();
        healthObject.OnDeath += OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        sugarCollectedText.GetComponent<TMPro.TextMeshProUGUI>().text = sugarCollected.ToString();
    }

    void OnDeath(GameObject gameobject)
    {
        sugarCollected = 0;
    }
}
