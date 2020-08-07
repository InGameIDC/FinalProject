using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] heoresIcons;

    private void Awake()
    {
        if(heoresIcons == null)
            heoresIcons = GameObject.FindGameObjectsWithTag("HeroIcon");
    }
    

    public void SetHeroesIcons(GameObject[] heroes, Action<GameObject> onIconClick)
    {
        Debug.Log("BB");
        for (int i = 0; i < heoresIcons.Length; i++)
        {
            HeroIcon heroIcon = heoresIcons[i].GetComponent<HeroIcon>();
            Debug.Log("I: " + i + ", Heores.Length: " + heroes.Length);
            if (heroes.Length > i && heroes[i] != null)
            {
                heoresIcons[i].SetActive(true);
                HeroUnit hero = heroes[i].GetComponent<HeroUnit>();
                Sprite heroImage = hero.getImage();
                heroIcon.setIconSprite(heroImage);
                heroIcon.hero = heroes[i];
                heroIcon.ActiveOnClick += onIconClick;
            }
            else
            {
                heoresIcons[i].SetActive(false);
            }
        }
    }
}
