using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script to maintain player stats and modifiers between scenes
public static class PlayerStats
{
    private static double hp = 3.0;

    public static double Hp
    {
        get
        {
            return hp;
        }
        set // will UPDATE the hp, not set the value outright
        {
            hp += value;
        }
    }

    
}
