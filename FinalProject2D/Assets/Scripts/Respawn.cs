using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// author : dor peretz
/// </summary>
public class Respawn : MonoBehaviour
{
    public static GameObject[] respawnHeroesPrefabsArray;
    public static GameObject[] respawnEnemiesPrefabsArray;
    public static Respawn Instance;
    private static int heroesLastPosIndex = 0;
    private static int enemiesLastPosIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        respawnHeroesPrefabsArray = GameObject.FindGameObjectsWithTag("RespawnHeroes");
        respawnEnemiesPrefabsArray = GameObject.FindGameObjectsWithTag("RespawnEnemies");
    }


    /// <summary>
    /// author: dor peretz
    /// respawns the unit at the random postion from the respawn gameObjet array
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="respawnTime"></param>
    public static IEnumerator DieAndRespawn(GameObject gameObject, float respawnTime)
    {
        gameObject.transform.position = new Vector2(0.5f, 4.5f);
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        gameObject.SetActive(true);
        HeroUnit unit = gameObject.GetComponent<HeroUnit>();
        gameObject.GetComponent<HeroUnit>().Start();


        gameObject.GetComponentInChildren<Health>().ResetHealth();
        if (gameObject.tag == "HeroUnit")
        {
            int posIndex = Random.Range(0, respawnHeroesPrefabsArray.Length);
            if (posIndex == heroesLastPosIndex)  // not to get the same location twice in a row
                posIndex = (posIndex + 1) % respawnHeroesPrefabsArray.Length;
            gameObject.transform.position = respawnHeroesPrefabsArray[posIndex].transform.position + new Vector3(Random.Range(-0.1f, -0.1f), Random.Range(-0.1f, -0.1f));
            heroesLastPosIndex = posIndex;
        }

        else
        {
            int posIndex = Random.Range(0, respawnEnemiesPrefabsArray.Length);
            if (posIndex == enemiesLastPosIndex)  // not to get the same location twice in a row
                posIndex = (posIndex + 1) % respawnEnemiesPrefabsArray.Length;

            gameObject.transform.position = respawnEnemiesPrefabsArray[posIndex].transform.position + new Vector3(Random.Range(-0.1f, -0.1f), Random.Range(-0.1f, -0.1f));
            enemiesLastPosIndex = posIndex;
        }
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
