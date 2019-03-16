using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlush : Item
{
    // Decoy will be destroyed when it runs out of health. 
    private int hp = 3;

    // Keep hold of a reference to all the enemies for use in updating target later.
    GameObject[] enemies;

    protected override void Start()
    {
        base.Start();
        // Can take three hits for the player regardless of damage done by the enemy. 
        hp = 3;
        
    }
    protected override void Pickup()
    {
        // If the player doesn't already have an active item, pick this one up.
        // Otherwise switch out the existing one. 
        //if (Stats.active == null)
        //{
        //    // Update the item charges and allow this to be used immediately.
        //    Stats.ActiveCharge = 3;
        //    Stats.CurrentCharge = 3;
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
        //else if (Stats.active != this)
        //{
        //    Switch();
        //    // Remove the text displays to prevent them from triggering on pickup since this item will be sticking around. 
        //    foreach (Transform child in transform)
        //    {
        //        if (child != transform)
        //            Destroy(child.gameObject);
        //    }
        //    Stats.ActiveCharge = 3;
        //    Stats.CurrentCharge = 3;
        //}
        
        if (Stats.active != this)
        {
            Stats.ActiveCharge = 3;
            Stats.CurrentCharge = 3;
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
            // Reset its health from any previous use.
            hp = 3;
            // 'Spawn' a decoy at the current tile the player stands at: reactivate the item and update its position. 
            gameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            gameObject.SetActive(true);
            // Instantiate(Resources.Load("CatPlush"), GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
            // Update the targets of all enemies to focus on the decoy while it's active. 
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies)
            {
                e.BroadcastMessage("UpdateTarget", gameObject.transform);
            }
        }
        
    }

    private void IsHit()
    {
        // Reduce the number of hits the decoy can take.
        hp--;
        // If the decoy is out of health, reset the target of all enemies to the player and destroy the decoy. 
        if (hp == 0)
        {
            foreach (GameObject e in enemies)
            {
                e.BroadcastMessage("UpdateTarget", GameObject.FindGameObjectWithTag("Player").transform);
            }
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reduce the number of hits the decoy can take.
        hp--;
        
        // If the decoy is out of health, reset the target of all enemies to the player and destroy the decoy. 
        if (hp == 0)
        {
            foreach (GameObject e in enemies)
            {
                e.BroadcastMessage("UpdateTarget", player.transform);
            }
            Destroy(gameObject);
        }
    }
}
