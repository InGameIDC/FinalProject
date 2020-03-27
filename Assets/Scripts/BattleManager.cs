using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    List<GameObject> gameObjects;
    [SerializeField] GameObject testHero;
    InputManager _inputManager;
    private HeroUnit _currentUnit;
    private Player[] _currentPlayers;

    // Start is called before the first frame update
    private void Awake()
    {
        initInputManager();
        _currentUnit = testHero.GetComponent<HeroUnit>();
    }

    #region InputManager manage functions
    private void initInputManager()
    {
        _inputManager = transform.GetComponent<InputManager>();
        _inputManager.OnUnitClick += OnHeroClicked;
        _inputManager.OnFieldClick += OnFieldClicked;
    }

    /// <summary>
    ///What happens when we click on a unit, should be changed to differentiate between 
    ///enemy and hero (with Unit actions).
    ///Author: Or Daniel.
    /// </summary>
    /// <param name="clickedobject"></param>

    private void OnHeroClicked(GameObject clickedobject)
    {
        Test.SetTargetColor(clickedobject, Color.magenta);
        //Unit clickedUnit = clickedobject.GetComponent<Unit>();
        if (clickedobject.tag.Equals("HeroUnit")) // && clickedUnit.owner != _currentUnit)
        {
            //change current unit if needed.
            _currentUnit = clickedobject.GetComponent<HeroUnit>();
        }
        else if (clickedobject.tag.Equals("EnemyUnit"))
        {
            //attack the selected enemy with current unit
            Debug.Log("current unit is attacking " + clickedobject);
            _currentUnit.SetTargetObj(clickedobject);
        }

        //This is a problem, as _currentUnit is a Unit script that has no reference
        //to the actual object it is attached to, for the time being.
        Debug.Log("current unit is attached to " + _currentUnit);
    }

    /// <summary>
    ///What happens when we click a field, should be changed so that it uses Move-to
    ///function.
    ///Author: Or Daniel.
    /// </summary>
    private void OnFieldClicked(Vector3 targetPosition)
    {
        Debug.Log("target position is " + targetPosition);
        _currentUnit.GoTo(targetPosition);
    }
    #endregion
}
