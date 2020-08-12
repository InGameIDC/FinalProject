using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// author : dor peretz
/// </summary>
public static class Respawn
{
    public static GameObject[] respawnHeroesPrefabsArray = GameObject.FindGameObjectsWithTag("RespawnHeroes");
    public static GameObject[] respawnEnemiesPrefabsArray = GameObject.FindGameObjectsWithTag("RespawnEnemies");

    /// <summary>
    /// author: dor peretz
    /// respawns the unit at the random postion from the respawn gameObjet array
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="respawnTime"></param>
    public static IEnumerator DieAndRespawn(GameObject gameObject, float respawnTime)
    {
        gameObject.transform.position =new Vector2(0.5f, 4.5f);
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        gameObject.SetActive(true);
        HeroUnit unit = gameObject.GetComponent<HeroUnit>();
        gameObject.GetComponent<HeroUnit>().Start();

        if (gameObject.tag == "HeroUnit")
        {
            gameObject.transform.position = respawnHeroesPrefabsArray.RandomItem().transform.position + new Vector3(Random.Range(-0.1f, -0.1f), Random.Range(-0.1f, -0.1f));
        }

        else
            gameObject.transform.position = respawnEnemiesPrefabsArray.RandomItem().transform.position + new Vector3(Random.Range(-0.1f, -0.1f), Random.Range(-0.1f, -0.1f));

        gameObject.GetComponentInChildren<Health>().ResetHealth();

    }


    /// <summary>
    /// author: dor peretz
    /// respawns the unit at a selected postion in vector 3.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="respawnPos"></param>
    /// <param name="respawnTime"></param>
    public static IEnumerator DieAndRespawnSpecialPosition(GameObject gameObject, Vector2 respawnPos, float respawnTime)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        gameObject.transform.position = respawnPos + new Vector2(Random.Range(-0.1f,-0.1f), Random.Range(-0.1f, -0.1f));
        gameObject.SetActive(true);
    }

    //public void dieAndRespawn(GameObject hero, float time) { StartCoroutine(DieAndRespawn(hero, time)); }
    //public void dieAndRespawnSpecialPosition(GameObject hero, Vector3 pos, float time) { StartCoroutine(DieAndRespawnSpecialPosition(hero, pos, time)); }

}
/// <summary>
///  This is an extension method. RandomItem() will now exist on all arrays.
/// </summary>
public static class ArrayExtensions
{

    public static T RandomItem<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

}
