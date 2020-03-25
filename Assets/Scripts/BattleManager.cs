using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    List<GameObject> gameObjects;
    [SerializeField] GameObject testScanner;
    [SerializeField] GameObject testHero;

    // Start is called before the first frame update
    void Start()
    {
        initScanner();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void initScanner()
    {
        Scanner scanner = testScanner.GetComponent<Scanner>();
        scanner.OnObjEnter += addObjToObjTargetsBank;
        scanner.OnObjExit += removeObjFromObjTargetsBank;
    }

    private void addObjToObjTargetsBank(GameObject attacker, GameObject target)
    {
        HeroUnit hero = attacker.GetComponent<HeroUnit>();
        hero.AddEnemyToBank(target);
        Test.SetTargetColor(target, Color.green);
    }

    private void removeObjFromObjTargetsBank(GameObject attacker, GameObject target)
    {
        HeroUnit hero = attacker.GetComponent<HeroUnit>();
        hero.RemoveEnemyFromBank(target);
        Test.SetTargetColor(target, Color.white);
    }


}
