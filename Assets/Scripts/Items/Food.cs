using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    protected override void Pickup()
    {
        // Add the item into the player's picked up passives. 
        Stats.passives.Add(this);
        // Remove the item from the item pool so it doesn't show up again. 
        //ItemManager.instance.RemoveFromPool(this);

        // Edit the player's health. 
        Stats.Heal(1);
    }
}
