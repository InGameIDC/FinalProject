using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  The class's purpose is to "listen" to everything that happens on the field via 
    Touch controls. The actual detection of taps happens via battle manager\player
    scritps, and activation (addition) of methods is done via BattleManager.

    Author: Or Daniel.

    */
public class InputManager
{
    /*  keep track via vector3 (for specific location on map)  for Field.
     *  via Unit for HeroClicked and HeroLongClick.   
         */

    public Action<Vector3> FieldClicked = delegate { };
    public Action<Unit> HeroClicked = delegate { };
    public Action<Unit> HeroLongClicked = delegate { };


}
