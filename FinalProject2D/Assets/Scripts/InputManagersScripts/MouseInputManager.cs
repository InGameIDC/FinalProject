﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/*  The class's purpose is detection of taps happening during battle
    and activation (addition to delegation) of methods to BattleManager.
    In short - detectection and translation of taps.

    Author: Or Daniel.

    */
public class MouseInputManager : MonoBehaviour
{
    public Action<GameObject> OnUnitDoubleClick = delegate { };
    public Action<GameObject> OnUnitClick = delegate { };
    public Action<Vector3> OnFieldClick = delegate { };


    //Defining a singleton for the InputManager.
    #region InputManager singleton
    private static MouseInputManager _instance;
    public static MouseInputManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<MouseInputManager>();
            return _instance;
        }
    }
    #endregion

    //Defining ray to detect objects clicked 
    private RaycastHit2D _hit;
    private GameObject _objectClicked;

    //Reference to the BattleManager
    //private BattleManager _battleManager;

    //Used to detect double taps
    private bool _wasClicked = false;
    private float _timeOfLastTouch = 0f;
    private float _maxDoubleTapTime = 0.3f;
    
    //In case we want the Unit to trigger double tap only if the two taps were near the
    //Unit itself, this should be used to detect the location of the touch.
    
    private Vector2 _lastTouchPosition;
    private float _maxDistance = 3f;

    private void Start()
    {
        //_battleManager = GameObject.FindObjectOfType<BattleManager>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            //Standard detection of taps and their location on screen.
            Vector3 position = Input.mousePosition;
            LayerMask maskLayersToRelate = LayerMask.GetMask(GlobalCodeSettings.Layers_Mouse_RayCast_Relate);
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero, Mathf.Infinity, maskLayersToRelate);

            //If we clicked on a collider object
            if (_hit.collider != null)
            {
                
                //Saves the game object that we interacted with.
                _objectClicked = _hit.collider.gameObject;
                float distanceFromLastClick = Vector2.SqrMagnitude(_lastTouchPosition - _hit.point);

                //Debugging

                //Debug.Log("Object clicked: " + _objectClicked +
                //            " in position: " + _hit.collider.transform.position.ToString() +
                //            ", impact point is: " + _hit.point.ToString()
                //            + "Distance from last touch is: " + distanceFromLastClick);

                if (!_wasClicked)
                {
                    _wasClicked = true;
                    StartCoroutine("SingleOrDouble");
                }       
                else if (_wasClicked
                    && (Time.time - _timeOfLastTouch <= _maxDoubleTapTime)
                    && distanceFromLastClick <= (_maxDistance * _maxDistance))
                {
                    Debug.Log("Double Touch detected");
                    DoubleClick(_objectClicked, _hit);
                    _wasClicked = false;
                }
                _timeOfLastTouch = Time.time;
                _lastTouchPosition = _hit.point;
            }
        }

    }
    IEnumerator SingleOrDouble()
    {
        yield return new WaitForSecondsRealtime(_maxDoubleTapTime);
        if (_wasClicked)
        {
            //Debug.Log("Single Touch detected");
            _wasClicked = false;
            SingleClick(_objectClicked, _hit);
            StopCoroutine("SingleOrDouble");
        }
    }

    private void SingleClick(GameObject collider, RaycastHit2D hit)
    {
        //Debug.Log("Single FUNCTION called with object " + collider + " on point " + hit.point);
        if (collider.tag.Equals("HeroUnit") || collider.tag.Equals("EnemyUnit"))
        {
            OnUnitClick(_objectClicked);
        } 
        else if (collider.tag.Equals("Terrain"))
        {
            OnFieldClick(hit.point);
        }
        return;
    }

    private void DoubleClick(GameObject collider, RaycastHit2D hit)
    {
        Debug.Log("Double FUNCTION called with object " + collider + " on point " + hit.point);
        if (collider.tag.Equals("HeroUnit"))
        {
            OnUnitDoubleClick(collider);
        }
    }
}
