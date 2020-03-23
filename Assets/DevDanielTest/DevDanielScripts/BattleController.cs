using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController 
{
    private InputManager _inputManager;
    private Unit _activeUnit;
    private Player[] _currentPlayers;


    private void StartBattle()
    {
        _inputManager.FieldClicked += OnFieldClicked;
        _inputManager.HeroClicked += OnHeroClicked;

    }

    private void OnHeroClicked(Unit clickedUnit)
    {
        Player unitOwner = clickedUnit.owner;
    }

    private void OnFieldClicked(Vector3 targetPosition)
    {

    }
}
