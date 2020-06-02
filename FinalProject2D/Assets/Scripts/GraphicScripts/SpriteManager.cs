using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SpriteManager : MonoBehaviour
{
    //[SerializeField] private Sprite heroSpriteRenderer;
    [SerializeField] private Shader shaderNoOutline;
    [SerializeField] private Shader shaderOutline;
    [SerializeField] public bool isSelectedOnStart = false;
    [SerializeField] private SpriteRenderer heroSpriteRenderer;


    void Start()
    {
        //heroSpriteRenderer.sprite = heroSprite;

        heroSpriteRenderer = GetComponent<SpriteRenderer>();
        if (isSelectedOnStart)
            EnableOutlineCharacter();
        else
            DisableOutlineCharacter();
    }

    public void EnableOutlineCharacter()
    {
        Shader currentShader = heroSpriteRenderer.material.shader;
        if (currentShader != null && currentShader != shaderOutline)
        {
            heroSpriteRenderer.material.shader = shaderOutline;
        }
    }

    public void DisableOutlineCharacter()
    {
        Shader currentShader = heroSpriteRenderer.material.shader;
        if (currentShader != null && currentShader != shaderNoOutline)
        {
            heroSpriteRenderer.material.shader = shaderNoOutline;
        }
    }

    //Used for blinking effect of selecting enemy units
    public IEnumerator ClickBlinkUnit()
    {
        //how many times the outline will appear and disappear to indicate enemy selected.
        const int NUMBER_OF_BLINKS = 3;
        const float TIME_BETWEEN_BLINKS = 0.1f;
        
        Debug.Log("I'm in the method");

        for (int i = 0; i < NUMBER_OF_BLINKS; i++)
        {
            this.EnableOutlineCharacter();
            Debug.Log("I ENABLE");
            yield return new WaitForSeconds(TIME_BETWEEN_BLINKS);
            Debug.Log("I WAIT");
            this.DisableOutlineCharacter();
            Debug.Log("I DISABLE");
            yield return new WaitForSeconds(TIME_BETWEEN_BLINKS);
        }

        Debug.Log("I'm finished");

    }

}
