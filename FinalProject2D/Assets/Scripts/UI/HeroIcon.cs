using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HeroIcon : MonoBehaviour
{
   [SerializeField] public GameObject hero = null;
    public Action<GameObject> ActiveOnClick;

    public void OnHeroIconClick()
    {
        if (hero != null)
            ActiveOnClick(hero);
    }

    public void setIconSprite(Sprite sprite)
    {
        Image iconImage = transform.GetChild(0).GetComponent<Image>();
        iconImage.sprite = sprite;
    }
}
