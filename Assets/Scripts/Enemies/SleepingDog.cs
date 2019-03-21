using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingDog : Enemy
{

    // Start is called before the first frame update
    void Awake()
    {
        // Empty. Will only take up its own tile on the board. 
        attackTargets = new List<Vector2Int> { };
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override List<Vector2Int> GetAttackTargets()
    {
        return new List<Vector2Int> { };
    }

    protected override void Attack()
    {
        // Nothing. He sleeps. 
        return;
    }
}
