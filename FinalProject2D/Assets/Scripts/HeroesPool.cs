using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heroes Pool", menuName = "Heroes Pool", order = 51)]
public class HeroesPool : ScriptableObject
{
    [SerializeField] List<GameObject> heroesPool = new List<GameObject>();

    public GameObject getHeroById(int id)
    {
        GameObject hero = null;
        try
        {
            hero =  heroesPool.Find(unit => unit.GetComponent<HeroUnit>().getId() == id);
        }
        catch(System.NullReferenceException err)
        {
            string errorStr = "Error, Could not find Hero with the id: " + id + "\n Heroes in the pool:\n";
            
            foreach (GameObject h in heroesPool)
            {
                errorStr += h.name + "\n";
            }

            errorStr += "\n Addition information:\n" + err;
            Debug.LogError(errorStr);
        }

        return hero;
    }
}
