using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOut : MonoBehaviour
{
     [SerializeField] public float FadeRate = 1f;
     private Image image;

    void Start()
    {
        image = GetComponentInChildren<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        
        float transpearency = color.a;
        if(transpearency > 0) {
            transpearency -= FadeRate * Time.deltaTime;
            if (transpearency > 0) {
                color.a = transpearency;
                image.color = color;
            }
            else{
                color.a = 0;
                image.color = color;
            }
        }
    }
}
