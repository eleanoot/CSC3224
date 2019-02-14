using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from base class for objects that can move. 
public abstract class Enemy : MonoBehaviour
{
    protected double hp;
    // How many hearts this enemy's attack knocks off the player.
    protected double damageDealt;
    // The squares this enemy aims for, if any.
    protected static List<Vector2> attackTargets;
    // The time it takes this Enemy to act. 
    protected float actionTime;
    protected float attackTimer = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Will be overridden by functons in the inherited classes that specialise the enemy type.
    protected abstract void Attack();

    // Update is called once per frame
    void Update()
    {
        
    }
}
