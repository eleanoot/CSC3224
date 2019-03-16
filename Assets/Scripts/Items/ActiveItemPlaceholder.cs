using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemPlaceholder : Item
{

    protected override void Pickup()
    {
        //// If the player doesn't already have an active item, pick this one up.
        //// Otherwise switch out the existing one. 
        //if (Stats.active == null)
        //{
        //    // Update the item charges and allow this to be used immediately.
        //    Stats.ActiveCharge = 4;
        //    Stats.CurrentCharge = 4;
        //    gameObject.SetActive(false);
        //    // Remove the text displays to prevent them from triggering on pickup since this item will be sticking around. 
        //    foreach (Transform child in transform)
        //    {
        //        if (child != transform)
        //            Destroy(child.gameObject);
        //    }
        //    Stats.ActiveItem = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Switch();
        //    Stats.ActiveCharge = 4;
        //    Stats.CurrentCharge = 4;
        //}
        if (Stats.active != this)
        {
            Stats.ActiveCharge = 4;
            Stats.CurrentCharge = 4;
            base.Pickup();
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnUse()
    {
        if (Stats.CurrentCharge == Stats.ActiveCharge)
        {
            // Reset the current amount of charge.
            Stats.CurrentCharge = 0;
            Debug.Log("Active placeholder used");
        }
        
    }
}
