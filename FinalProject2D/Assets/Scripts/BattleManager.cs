using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    List<GameObject> gameObjects;
    [SerializeField] GameObject testHero;
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
        _currentUnit = testHero.GetComponent<HeroUnit>();
    }
    private void Start()
    {
        //initInputManager();
        initInputMouseManager();

        //follow the score
        GameObject hill = GameObject.FindGameObjectWithTag("Hill");
        hill.GetComponent<Hill>().OnScoreChange += ScoreUpdate;

        SpriteManager currentUnitSpriteManager = _currentUnit.gameObject.GetComponentInChildren<SpriteManager>();
        if (currentUnitSpriteManager != null)
            currentUnitSpriteManager.EnableOutlineCharacter();

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
            SpriteManager prevUnitSpriteManager = _currentUnit.gameObject.GetComponentInChildren<SpriteManager>();
            _currentUnit = clickedobject.GetComponent<HeroUnit>();

            if (prevUnitSpriteManager != null)
                prevUnitSpriteManager.DisableOutlineCharacter();

            SpriteManager currentUnitSpriteManager = _currentUnit.gameObject.GetComponentInChildren<SpriteManager>();
            if (prevUnitSpriteManager != null)
                currentUnitSpriteManager.EnableOutlineCharacter();

        }
        //If we're dealing with an EnemyUnit.
        else if (clickedobject.tag.Equals("EnemyUnit") && _currentUnit != null) 
        {

            _currentEnemyClicked = clickedobject.GetComponent<HeroUnit>();
            _currentUnit.SetTargetObj(clickedobject);

            SpriteManager enemySpriteManager = clickedobject.GetComponentInChildren<SpriteManager>();

            //If an enemy was selected before and the indication didn't end - we don't stop it, as blink is temporary.
            //Start indication on new enemy.
            if (enemySpriteManager != null)
                StartCoroutine(enemySpriteManager.ClickBlinkUnit());
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
        StartCoroutine(Test.MarkCircleAtPos(new Vector3(targetPosition.x, targetPosition.y, -0.1f), 0.3f, 0.3f, 0.025f, Color.white));
        //Debug.Log("target position is " + targetPosition);
        _currentUnit.GoTo(targetPosition);
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
