using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsInit : MonoBehaviour
{
    [SerializeField] private HeroesPool heroesData;
    //[SerializeField] private HeroesPool enemiesData;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] heroesStartPoses = GameObject.FindGameObjectsWithTag("StartPosHeroes");
        //GameObject[] enemiesStartPoses = GameObject.FindGameObjectsWithTag("StartPosEnemies");
        GameObject[] heroesPrefs = null;

        try
        {
            GameStatus gameStatus = GameObject.Find("GameStatus").GetComponent<GameStatus>(); // get the player selection
            heroesPrefs = getUnitsPrefs(heroesData, gameStatus.deckPlayers); // geting the units prefs
        }
        catch (System.NullReferenceException err)
        {
            Debug.LogError(err);
        }

        GameObject[] heroes = initUnits(heroesPrefs, heroesStartPoses);
        BattleManager.Instance.setHeroes(heroes);
    }

    /// <summary>
    /// Init the units according to the given positions, randomly.
    /// Ilan.
    /// </summary>
    /// <param name="units"></param>
    /// <param name="startPoses"></param>
    /// <returns>The inited units</returns>
    private GameObject[] initUnits(GameObject[] unitsPrefs, GameObject[] startPoses)
    {
        int unitsFirstIndex = Random.Range(0, unitsPrefs.Length);
        int startPosFirstIndex = Random.Range(0, startPoses.Length);
        List<GameObject> units = new List<GameObject>();

        for (int i = 0; i < unitsPrefs.Length; i++)
        { // init units in the poses, with the random start index
            if (unitsPrefs[(unitsFirstIndex + i) % unitsPrefs.Length] != null)
            {
                Transform posTransform = startPoses[(startPosFirstIndex + i) % startPoses.Length].transform;
                units.Add(Instantiate(unitsPrefs[(unitsFirstIndex + i) % unitsPrefs.Length], posTransform.position, posTransform.rotation));
            }
            else
                Debug.LogError("Could not find a hero with id: " + unitsPrefs[(unitsFirstIndex + i) % unitsPrefs.Length]);
        }

        return units.ToArray();
    }

    /// <summary>
    /// Return the an heroes array according to the given ids.
    /// Ilan.
    /// </summary>
    /// <param name="unitsPool">Units to search by id</param>
    /// <param name="unitsIds">Selected ids/param>
    /// <returns></returns>
    public GameObject[] getUnitsPrefs(HeroesPool unitsPrefsPool, int[] unitsIds)
    {
        GameObject[] units = new GameObject[unitsIds.Length];

        for (int i = 0; i < unitsIds.Length; i++)
        {
            units[i] = unitsPrefsPool.getHeroById(unitsIds[i]);
        }

        return units;
    }
}
