using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HeroIcon : MonoBehaviour
{
   [SerializeField] private GameObject _hero = null;
    public Action<GameObject> ActiveOnClick;

    public void OnHeroIconClick()
    {
        if (_hero != null)
            ActiveOnClick(_hero);
    }

    public void setIconSprite(Sprite sprite)
    {
        Image iconImage = transform.GetChild(0).GetComponent<Image>();
        iconImage.sprite = sprite;
    }

    private void setColor(Color color)
    {
        Image iconImage = GetComponent<Image>();
        iconImage.color = color;
    }

    public void setIconToSelectedColor()
    {
        setColor(UIData.instance.getSelectedHeroIconColor());
    }
    public void setIconToRegularColor()
    {
        setColor(UIData.instance.getHeroIconColor());
    }

    public void setIconToUnavailableColor()
    {
        setColor(UIData.instance.getHeroNotAvailableIconColor());
    }

    public void setHero(GameObject newHero)
    {
        _hero = newHero;
        _hero.GetComponentInChildren<Health>().OnDeath += OnHeroNotAvailiable;
    }

    public GameObject getHero() => _hero;

    private void OnHeroNotAvailiable(GameObject hero)
    {
        setIconToUnavailableColor();
        hero.GetComponentInChildren<Health>().OnDeath -= OnHeroNotAvailiable;
        hero.GetComponentInChildren<HeroUnit>().OnRespawn += OnHeroAvailiable;
    }

    private void OnHeroAvailiable(GameObject hero)
    {
        setIconToRegularColor();
        hero.GetComponentInChildren<Health>().OnDeath += OnHeroNotAvailiable;
        hero.GetComponentInChildren<HeroUnit>().OnRespawn -= OnHeroAvailiable;
    }

}
