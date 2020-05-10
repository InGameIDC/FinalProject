using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Author:OrS
/// attached to the chosen hero panel
/// this class helps with transfering infrormation from the chosen hero cards in the deck and the hero cards in the collection
/// since there are 3 deck cards and 9+ hero cards this class manages the switching and updating
/// </summary>
public class ChangeHero : MonoBehaviour
{
    //-------hero in change process details-------//
    public int inChangeId;
    public int inChangeLevel;
    public int inChangeFamily;
    public bool inChangeProcess = false;

    //-------deligated functions-------//
    public Action<int> turnOffInUse = delegate { };         //lets the hero cards know that a hero was swiched in the deck and the "old" hero needs to change status
    public Action<int> finishChange = delegate { };         //lets the hero cards know that the change of the cards in the deck was completed
    public Action<bool> inUseOutlineOn = delegate { };      //lets know that the outlines should be disabled/enabled  
    public Action<int, int> cardUpgrade = delegate { };

}
