using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //Declaring delegates:
    public Action<GameObject> onUnitDoubleClick = delegate { };
    public Action<GameObject> onUnitClick = delegate { };
    public Action<Vector3> onFieldClick = delegate { };


    private Unit _currentUnit;
    private Player[] _currentPlayers;


    private void Start()
    {
        onUnitClick += OnHeroClicked;
        onFieldClick += OnFieldClicked;
    }

    //What happens when we click on a unit, should be changed to differentiate between 
    //enemy and hero (with Unit actions).
    private void OnHeroClicked(GameObject clickedobject)
    {
        Unit clickedUnit = clickedobject.GetComponent<Unit>();
        if (clickedobject.tag.Equals("HeroUnit")  && clickedUnit.owner != _currentUnit)
        {
            //change current unit if needed.
            _currentUnit = clickedUnit;
        }
        else if (clickedobject.tag.Equals("EnemyUnit"))
        {
            //attack the selected enemy with current unit
            Debug.Log("current unit is attacking " + clickedobject);
        }

        //This is a problem, as _currentUnit is a Unit script that has no reference
        //to the actual object it is attached to, for the time being.
        Debug.Log("current unit is attached to " + _currentUnit);        
    }

    //What happens when we click a field, should be changed so that it uses Move-to
    //function.
    private void OnFieldClicked(Vector3 targetPosition)
    {
        Debug.Log("target position is " + targetPosition);
    }
}
