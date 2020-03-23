using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  
  This class is used to control and keep track of Units on the battle field.
  Each unit has a reference to its "Owner", aka the Player\Hero it is associated
  to, which allows it to get all the needed information (stats\skills) from it.
  
  Author: Or Daniel. 

*/

public class Unit
{

    //  To be ignored ATM, as this might be triggered directly from Player.
    //
    //  public Action<Unit> Dead = delegate { };
    //  public Action<Unit, int> Hit = delegate { };

    /*  Player which the unit inherits everything from for the current battle.
        We allow get only to be able to set Heroes in the beginning of
        the battle (on initialization, and get the owner of the current unit 
        to execute methods like Move, Attack etc.
    */
    public Player owner { get; }

    //Initially this is the spawn location of the Unit.
    private Vector3 _currentPosition;

    public Unit(Player unitOwner) {

        owner = unitOwner;
    
    }


    /*  This is probably going to be triggered directly from Player class.
     
      
    public void Move(Vector3 newPosition) { 
    
    }

    public void TakeDamage(int damageValue) { 
    
    }

    */
}
