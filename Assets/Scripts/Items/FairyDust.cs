using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDust : Item
{
    protected override void Pickup()
    {
        // Add the item into the player's picked up passives. 
        Stats.passives.Add(this);
        // Edit the player's attack range. 
        Stats.Range *= 2;
        base.Pickup();
    }
    
}
