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
    public static GameObject[] respawnPrefabsArray = GameObject.FindGameObjectsWithTag("Respawn");

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
        gameObject.GetComponent<HeroUnit>().Start();
        gameObject.transform.position = respawnPrefabsArray.RandomItem().transform.position;

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
        gameObject.transform.position = respawnPos;
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
