using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    List<GameObject> gameObjects;
    [SerializeField] private ControlPointsManager _ctrlPointsManager;
    [SerializeField] GameObject[] Heores;
    //private TouchInputManager _TouchInputManager;
    private MouseInputManager _MouseInputManager;
    [SerializeField] GameObject _inputManager;
    private HeroUnit _currentUnit;
    private HeroUnit _currentEnemyClicked;

    // for loading next level and some data
    GameObject gs;

    // For Score System
    public float gameScore;
    public Action<int> onGameScoreEnd = delegate { };
    public GameObject egp;


    // Start is called before the first frame update
    private void Awake()
    {
        gs = GameObject.FindGameObjectWithTag("GameStatus");
        _currentUnit = Heores[0].GetComponent<HeroUnit>();
    }
    private void Start()
    {
        //initInputManager();
        initInputMouseManager();

        //follow the score
        GameObject sm = GameObject.FindGameObjectWithTag("SugarManager");
        sm.GetComponent<SugarManager>().OnScoreChange += ScoreUpdate;

        selectANewHero(_currentUnit.gameObject);
    }

    private void selectANewHero(GameObject prevHero)
    {
        Health prevHeroHealth = prevHero.GetComponentInChildren<Health>();
        GameObject newHero = getRandomHero();

        if (newHero != null){

            Health newHeroHealth = newHero.GetComponentInChildren<Health>();

            try { prevHeroHealth.OnDeath -= selectANewHero; }
            catch (Exception ex) { Debug.Log(ex); }

            _currentUnit = newHero.GetComponent<HeroUnit>();
            newHeroHealth.OnDeath += selectANewHero;

            markChange(prevHero, newHero);
        }
        else
        {
            StartCoroutine(waitForNewHeroesRespawnToBeSelected(prevHero));
        }
    }

    private GameObject getRandomHero()
    {
        foreach(GameObject hero in Heores)
        {
            if (hero.activeSelf)
                return hero;
        }

        return null;
    }

    private IEnumerator waitForNewHeroesRespawnToBeSelected(GameObject prevHero)
    {
        yield return new WaitForSeconds(0.1f);
        selectANewHero(prevHero);
    }

    /// <summary>
    /// change the prev unit to non-selected and the NEW current unit to selected.
    /// </summary>
    /// <param name="prevHero">The previous hero that was under control</param>
    /// <param name="newHero">The new hero that was selected to be controled</param>
    private void markChange(GameObject prevHero, GameObject newHero)
    {
        SpriteManager prevUnitSpriteManager = prevHero.GetComponentInChildren<SpriteManager>();
        try
        {
            if (prevUnitSpriteManager != null)
                prevUnitSpriteManager.DisableOutlineCharacter();
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        try
        {
            SpriteManager currentUnitSpriteManager = newHero.gameObject.GetComponentInChildren<SpriteManager>();
            if (prevUnitSpriteManager != null)
                currentUnitSpriteManager.EnableOutlineCharacter();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    #region InputManager manage functions

    /*
    private void initInputManager()
    {
        _TouchInputManager = _TouchInputManager.GetComponent<TouchInputManager>();
        _TouchInputManager.OnUnitClick += OnHeroClicked;
        _TouchInputManager.OnFieldClick += OnFieldClicked;
        _TouchInputManager.OnUnitDoubleClick += OnHeroDoubleClick;
    }
    */
    private void initInputMouseManager()
    {
        _MouseInputManager = _inputManager.GetComponent<MouseInputManager>();
        _MouseInputManager.OnUnitClick += OnHeroClicked;
        _MouseInputManager.OnFieldClick += OnFieldClicked;
        _MouseInputManager.OnUnitDoubleClick += OnHeroDoubleClick;
    }


    /// <summary>
    ///What happens when we click on a unit, should be changed to differentiate between 
    ///enemy and hero (with Unit actions).
    ///Author: Or Daniel.
    /// </summary>
    /// <param name="clickedobject"></param>

    private void OnHeroClicked(GameObject clickedobject)
    {
        //If we're dealing with a HeroUnit.
        if (clickedobject.tag.Equals("HeroUnit"))
        {
            //If the unit is already selected - we don't change anything.
            if (_currentUnit.gameObject == clickedobject)
                return;

            //change the last unit to non-selected and the NEW current unit to selected.
            
            GameObject prevHero = _currentUnit.gameObject;
            _currentUnit = clickedobject.GetComponent<HeroUnit>();
            markChange(prevHero, clickedobject);
        }
        //If we're dealing with an EnemyUnit.
        else if (clickedobject.tag.Equals("EnemyUnit") && _currentUnit != null && _currentUnit.gameObject.activeSelf)
        {

            _currentEnemyClicked = clickedobject.GetComponent<HeroUnit>();
            //_currentUnit.SetTargetObj(clickedobject);
            // if had enough control points, show indication
            if (_ctrlPointsManager.CommandSetTargetToAttack(_currentUnit, clickedobject, false))
            {
                SpriteManager enemySpriteManager = clickedobject.GetComponentInChildren<SpriteManager>();

                //If an enemy was selected before and the indication didn't end - we don't stop it, as blink is temporary.
                //Start indication on new enemy.
                if (enemySpriteManager != null)
                    StartCoroutine(enemySpriteManager.ClickBlinkUnit());
            }
        }

        //This is a problem, as _currentUnit is a Unit script that has no reference
        //to the actual object it is attached to, for the time being.
        //Debug.Log("current unit is attached to " + _currentUnit);
    }

    /// <summary>
    ///What happens when we click a field, should be changed so that it uses Move-to
    ///function.
    ///Author: Or Daniel.
    /// </summary>
    private void OnFieldClicked(Vector3 targetPosition)
    {
        //Debug.Log("target position is " + targetPosition);
        //_currentUnit.GoTo(targetPosition);
        // if had enough control points, show indication
        if(_currentUnit.gameObject.activeSelf && _ctrlPointsManager.CommandyGoTo(_currentUnit, targetPosition, false))
        {
            StartCoroutine(Test.MarkCircleAtPos(new Vector3(targetPosition.x, targetPosition.y, -0.1f), 0.3f, 0.3f, 0.025f, Color.white));
        }
    }

    private void OnHeroDoubleClick(GameObject clickedobject)
    {
        if (clickedobject.GetComponent<HeroUnit>() == _currentUnit)
        {
            StartCoroutine(Test.MarkCircleAtPos(new Vector3(clickedobject.transform.position.x, 0f, clickedobject.transform.position.z), 0.3f, 0.4f, 0.025f, Color.green));
            _currentUnit.CancelOrders();
        }
        else
            OnHeroClicked(clickedobject);
    }
    #endregion

    public void DieAndRespawn(GameObject hero)
    {
        StartCoroutine(Respawn.DieAndRespawn(hero, 3f));
    }
    public void DieAndRespawnSpecialPosition(GameObject hero, Vector2 pos, float time) { StartCoroutine(Respawn.DieAndRespawnSpecialPosition(hero, pos, time)); }

    /// <summary>
    /// Author:OrS
    /// follow the score of the game, and in case of winning/loosing according to the score deligate that the game has ended
    /// </summary>
    /// <param name="newScore"></param>
    private void ScoreUpdate(float newScore)
    {
        gameScore = newScore;
        //Debug.Log(gameScore);

        if(gameScore == 100 || gameScore ==-100)
        {
            egp.SetActive(true);
            onGameScoreEnd((int)gameScore);
        }

    }

    #region end Battle Options

    /// <summary>
    /// Author: OrS
    /// loads the Home menu upon clicking go to menu button
    /// </summary>
    public void goToMenu()
    {
        SceneManager.LoadScene("HomeMenu");
    }

    /// <summary>
    /// Author: OrS
    /// loads the next level upon clicking go to next level
    /// </summary>
    public void goToNextLevel()
    {
        int idx = SceneManager.GetActiveScene().buildIndex + 1;

        if(idx > 5)
        {
            idx = 5;
        }
        gs.GetComponent<GameStatus>().lastLevelCosen = idx;
        SceneManager.LoadScene("ChooseHeroes");
    }

    /// <summary>
    /// Author: OrS
    /// loads the level again upon clicking restart button
    /// </summary>
    public void restartLevel()
    {
        SceneManager.LoadScene("ChooseHeroes");
    }

    #endregion

}
