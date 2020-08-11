using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] heoresIcons;
    [SerializeField] UIData uiData;

    private void Awake()
    {
        if(heoresIcons == null)
            heoresIcons = GameObject.FindGameObjectsWithTag("HeroIcon");

        if (UIData.instance == null && uiData != null) // setting the data
            UIData.instance = uiData;
    }
    

    public void SetHeroesIcons(GameObject[] heroes, Action<GameObject> onIconClick, GameObject selectedHero)
    {
        for (int i = 0; i < heoresIcons.Length; i++)
        {
            HeroIcon heroIcon = heoresIcons[i].GetComponent<HeroIcon>();
            if (heroes.Length > i && heroes[i] != null)
            {
                heoresIcons[i].SetActive(true);
                HeroUnit hero = heroes[i].GetComponent<HeroUnit>();
                Sprite heroImage = hero.getImage();
                heroIcon.setIconSprite(heroImage);
                heroIcon.setHero(heroes[i]);
                heroes[i].GetComponent<HeroUnit>().OnUnitSelect += SetSelectColor; // Assigns the icon colors manage
                heroIcon.ActiveOnClick += onIconClick; // Add functionallity, used by BattleManager to select the hero according to the icon that was clicked
                heroIcon.ActiveOnClick += SetSelectColor;
                
                // sets icon colors
                if(selectedHero != null && selectedHero == heroes[i])
                    heroIcon.setIconToSelectedColor();
                else
                    heroIcon.setIconToRegularColor();
            }
            else
            {
                heoresIcons[i].SetActive(false);
            }
        }
    }

    public void SetSelectColor(GameObject selectedHero)
    {
        foreach(GameObject icon in heoresIcons)
        {
            HeroIcon heroIcon = icon.GetComponent<HeroIcon>();
            if (heroIcon.getHero() == selectedHero)
                heroIcon.setIconToSelectedColor();
            else if (heroIcon.getHero().activeSelf)
                heroIcon.setIconToRegularColor();
            else
                heroIcon.setIconToUnavailableColor();
        }
    }

}
